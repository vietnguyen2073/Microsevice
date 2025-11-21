//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using PaymentService.Data;
//using System.IO;

//namespace PaymentService.Data
//{
//    public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
//    {
//        public PaymentDbContext CreateDbContext(string[] args)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory()) // thư mục đang chạy lệnh dotnet ef
//                .AddJsonFile("appsettings.json")
//                .Build();

//            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
//            var connectionString = configuration.GetConnectionString("Default");
//            optionsBuilder.UseNpgsql(connectionString);

//            return new PaymentDbContext(optionsBuilder.Options);
//        }
//    }
//}
