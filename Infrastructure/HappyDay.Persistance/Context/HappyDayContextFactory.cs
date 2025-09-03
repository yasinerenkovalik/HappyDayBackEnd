using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HappyDay.Persistance.Context
{
    public class HappyDayContextFactory : IDesignTimeDbContextFactory<HappyDayContext>
    {
        public HappyDayContext CreateDbContext(string[] args)
        {
            // API projesindeki appsettings.* dosyalarını okuyalım
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Presentation", "HappyDay.Api");
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("DefaultConnection");
            var builder = new DbContextOptionsBuilder<HappyDayContext>();
            // Postgres örneği:
            builder.UseNpgsql(cs);

            return new HappyDayContext(builder.Options);
        }
    }
}