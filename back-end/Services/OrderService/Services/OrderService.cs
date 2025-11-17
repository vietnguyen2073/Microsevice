using OrderService.DTOs.OrderDTOs;
using OrderService.DTOs.OrderItemDTOs;
using OrderService.Events.Interface;
using OrderService.Events.Models;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _repo;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public OrderService(IOrdersRepository repo, IRabbitMQPublisher rabbitMQPublisher)
        {
            _repo = repo;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllAsync()
        {
            var orders = await _repo.GetAllAsync();

            var orderDtos = orders.Select(order => new OrderReadDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemReadDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Amount = item.Amount,
                    TotalPrice = item.Price * item.Amount
                }).ToList()
            });


            return orderDtos;
        }

        public async Task<OrderReadDto?> GetByIdAsync(Guid id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderReadDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemReadDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Amount = item.Amount,
                    TotalPrice = item.Price * item.Amount
                }).ToList()
            };
        }

        public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto)
        {
            var order = new Order
            {
                CustomerName = dto.CustomerName,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Description = i.Description,
                    Price = i.Price,
                    Amount = i.Amount,
                    ImageUrl = i.ImageUrl
                }).ToList()
            };

            order.TotalPrice = order.OrderItems.Sum(i => i.Price * i.Amount);

            await _repo.CreateAsync(order);
            await _repo.SaveChangesAsync();

            var result = new OrderReadDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemReadDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Amount = item.Amount,
                    TotalPrice = item.Price * item.Amount
                }).ToList()
            };


            //RabbitMQ
            var orderEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                Amount = order.TotalPrice
            };

            _rabbitMQPublisher.Publish(orderEvent, exchangeName: "order_exchange", routingKey: "order.created");



            return result;
        }

        public async Task<bool> UpdateAsync(Guid id, OrderUpdateDto dto)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return false;

            order.Status = dto.Status;

            await _repo.UpdateAsync(order);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return false;

            await _repo.DeleteAsync(order);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
