using Microsoft.EntityFrameworkCore;
using RepositoryPattrenPractice.Models;

namespace RepositoryPattrenPractice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Student> Student { get; set; }
    }


}
