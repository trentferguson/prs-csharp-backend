using Microsoft.EntityFrameworkCore;

namespace PrsBackEnd.Models
{
    public class PrsDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Vendor> Vendors { get; set; }


        // constructor to support dependency injection
        public PrsDbContext(DbContextOptions<PrsDbContext> options) : base(options)
        {
          
        }

    }
}
