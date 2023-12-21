using Fx_converter.Models;
using Microsoft.EntityFrameworkCore;

namespace Fx_converter.Entities
{
    public class FxDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public FxDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
