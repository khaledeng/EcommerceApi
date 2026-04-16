using Day1.Models;
using Microsoft.EntityFrameworkCore;

namespace Day1.data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Product>(Entity =>
            {
                Entity.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);


                Entity.HasOne(p => p.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SupplierId);

            });
       



           
        }


    }
}
