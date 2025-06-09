using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShipeX2.Identity.Context;
using static ShipeX2.Identity.Context.ApplicationDbContext;

namespace ShipeX2.Identity.Services
{
    public class DynamicDbContextFactory : IDynamicDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DynamicDbContextFactory ( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext ( string clientId )
        {
            string dbName = $"{clientId}";
            string baseConnection = _configuration.GetConnectionString("DynamicDb");
            string finalConnection = baseConnection.Replace("{DB_NAME}", dbName);
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(finalConnection);
            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public ApplicationDbContext CreateDbContextOracle ( string clientId )
        {
            string dbService = $"{clientId}";
            string baseConnection = _configuration.GetConnectionString("DynamicDb");
            string finalConnection = baseConnection.Replace("{SERVICE_NAME}", dbService);
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseOracle(finalConnection);
            return new ApplicationDbContext(optionsBuilder.Options);
        }


    }
}
