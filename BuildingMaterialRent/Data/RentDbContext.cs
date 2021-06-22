using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuildingMaterialRent.Data
{
    public class RentDbContext : IdentityDbContext<User, Role, string>
    {
        public RentDbContext(
            DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .Property(p => p.Image)
                .HasDefaultValue("missing.jpg");

            base.OnModelCreating(builder);
        }
    }
}
