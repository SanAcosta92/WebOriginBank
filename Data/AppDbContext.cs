using WebOriginBank.Models;
using Microsoft.EntityFrameworkCore;

namespace WebOriginBank.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarjeta> Tarjeta { get; set; }
        public DbSet<Operacion> Operacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarjeta>()
                .HasIndex(t => t.Nro)
                .IsUnique();
        }
    }
}
