using Microsoft.EntityFrameworkCore;
using MyFirstApp.Entities;

namespace MyFirstApp.Data
{
	public class MyDbContext: DbContext
	{
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
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
