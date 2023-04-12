using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FreshShop.Data.EF
{  
    public class FreshShopDbContextFactory : IDesignTimeDbContextFactory<FreshShopDbContext>
    {
        public FreshShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()                
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("FreshShopSolutionDb");

            var optionsBuilder = new DbContextOptionsBuilder<FreshShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FreshShopDbContext(optionsBuilder.Options);
        }
    }
}
