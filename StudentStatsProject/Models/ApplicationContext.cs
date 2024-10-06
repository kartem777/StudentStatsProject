using Microsoft.EntityFrameworkCore;

namespace StudentStatsProject.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
