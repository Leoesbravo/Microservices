using Microsoft.EntityFrameworkCore;

namespace MicroserviceCupon.Model
{
    //Inyeccion de dependencias
    //Patrones de diseño
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Cupon> Cupones { get; set; }
    }
}
