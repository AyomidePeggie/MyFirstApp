using Microsoft.AspNetCore.Identity;
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
			var hasher = new PasswordHasher<ApplicationUser>();

			//create a role
			modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN" });

			//create a user
			modelBuilder.Entity<ApplicationUser>().HasData(
			   new ApplicationUser
			   {
				   Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
				   UserName = "ecommerceadmin",
				   NormalizedUserName = "ECOMMERCEADMIN",
				   PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),
				   FirstName = "Admin",
				   LastName = "Admin"

			   }
			   );

			//asign admin role to the user we created
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
			{
				RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
				UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"

			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
