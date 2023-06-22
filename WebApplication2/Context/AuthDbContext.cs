using Microsoft.EntityFrameworkCore;
using WebApplication2.Modelos;

namespace WebApplication2.Context
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        // Define las entidades que deseas utilizar en tu base de datos
        public DbSet<LoginModel> Administradores { get; set; }
        // ...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define las configuraciones de tus entidades si es necesario
            // modelBuilder.ApplyConfiguration(new LoginModelConfiguration());
            // ...
        }
    }
}
