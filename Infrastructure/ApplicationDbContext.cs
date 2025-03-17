using Microsoft.EntityFrameworkCore;
using RepositoryTutorial.Domain;

namespace RepositoryTutorial.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // -- add DbSets here for new application entities
        public DbSet<Product> Products { get; set; }
    }
}
