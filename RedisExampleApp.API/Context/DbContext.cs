using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Laptop", Price = 100 },
                new Product() { Id = 2, Name = "Desk", Price = 200 },
                new Product() { Id = 3, Name = "Lamb", Price = 300 }

            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
