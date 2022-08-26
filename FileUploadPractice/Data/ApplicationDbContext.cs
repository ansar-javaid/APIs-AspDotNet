using FileUploadPractice.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadPractice.Data
{
    public class ApplicationDbContext : DbContext   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }public DbSet<Document> Document { get; set; }
         
    }
}
