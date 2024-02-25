using Microsoft.EntityFrameworkCore;
namespace L01_2021_601.Models
{
    public class blogContext : DbContext
    {
        public blogContext(DbContextOptions<blogContext> options): base(options) {

        }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Publicaciones> Publicaciones { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }
    }
}
