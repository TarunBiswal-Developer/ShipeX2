using Microsoft.EntityFrameworkCore;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        //Interface For Connecting to different databases dynamically 
        public interface IDynamicDbContextFactory
        {
            ApplicationDbContext CreateDbContext ( string clientId );
        }

        //DbSet here
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<LoginCredential> LoginCredentials { get; set; }
        public DbSet<ShipCarrier> ShipCarriers { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<ShipXUser> ShipXUsers { get; set; }

    }
}
