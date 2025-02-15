using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Entities;
using MyFirstApp.Entities.Identity;

namespace MyFirstApp.Data
{
	public class MyDbContext: IdentityDbContext <ApplicationUser>
	{
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasKey(p => p.Id);


			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category) //product has one category 
				.WithMany(c => c.Products) // category has many products 
				.HasForeignKey(p => p.CategoryId) //foreign key 
				.OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(modelBuilder);
		}
	}
}
