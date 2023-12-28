using Fx_converter.Models;
using Microsoft.EntityFrameworkCore;

namespace Fx_converter.Entities
{
    public class FxDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public FxDbContext(DbContextOptions options) : base(options)
        {

        }
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.Symbol)
                .IsUnique();
            modelBuilder.Entity<Observation>()
                .HasIndex(o => o.Date)
                .IsUnique();
			base.OnModelCreating(modelBuilder);
		}
	}
}
