using Microsoft.EntityFrameworkCore;
using static ShipeX2.Persistence.TableModels.Tables;

namespace ShipeX2.Identity.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
            // Enable legacy timestamp behavior in Npgsql.
            // This makes Npgsql use the old mapping of PostgreSQL 'timestamp without time zone'
            // to DateTimeKind.Utc (instead of DateTimeKind.Unspecified as in newer versions).
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // Disable automatic conversion of PostgreSQL 'infinity' and '-infinity' date/time values.
            // Without this, Npgsql would map 'infinity' to DateTime.MaxValue and '-infinity' to DateTime.MinValue.
            // Disabling it ensures stricter handling and throws an exception instead of converting.
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
        public DbSet<Importer> Importers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CarrierService> CarrierServices { get; set; }
        public DbSet<CarrierPacking> CarrierPackings { get; set; }
        public DbSet<ServicePack> ServicePacks { get; set; }
        public DbSet<ClientCarrier> ClientCarriers { get; set; }

    }
}
