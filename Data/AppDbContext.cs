using Microsoft.EntityFrameworkCore;

namespace proyectoSistemaPropietarios.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Impresora> Impresoras { get; set; }
        public DbSet<Filamento> Filamentos { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
