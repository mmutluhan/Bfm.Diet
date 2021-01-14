using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bfm.Diet.Model
{
    public class DietDbContextFactory : IDesignTimeDbContextFactory<DietDbContext>
    {
        public DietDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DietDbContext>();
            var connectionString = configuration.GetSection("AppSettings:ConnectionStrings:DefaultConnection").Value;
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(connectionString);
            optionsBuilder.UseNpgsql(connectionString);
            return new DietDbContext(optionsBuilder.Options);
        }
    }
}