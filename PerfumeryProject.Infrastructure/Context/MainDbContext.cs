using Microsoft.EntityFrameworkCore;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.Infrastructure.Context
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Parfum> Parfums { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasMany(x => x.Parfums)
                .WithOne(y => y.BrandName)
                .HasForeignKey(x => x.BrandId)
                .IsRequired();


            base.OnModelCreating(modelBuilder);
        }
    }
}
