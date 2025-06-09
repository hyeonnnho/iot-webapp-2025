using Microsoft.EntityFrameworkCore;

namespace WebApi02.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        // 가장 중요!
        public DbSet<Book> Book { get; set; }
    }
}
