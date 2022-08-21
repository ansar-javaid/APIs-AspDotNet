using Microsoft.EntityFrameworkCore;
using ProfileAPI.Models;

namespace ProfileAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Car> Car { get; set; }
    }
}
