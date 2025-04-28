using CustomerManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<FeeRecord> FeeRecords { get; set; }
        public DbSet<FeeHistory> FeeHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👇 Add this line here
            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
