using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Data;
using MyFirstApp.Entities;

namespace MyFirstApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyDbContext _dbContext;
        public ProductsController(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllProducts()
        {
            List<Product> products = new List<Product>();
            products.Add(new Product
            {
                Id = 1,
                Name = "Hermes Bag",
                Description = "Fashion accessories",
                UnitPrice = 50000m
            });

            products.Add(new Product
            {
                Id = 2,
                Name = "Louis Wristwatch",
                Description = "Fashion accessories",
                UnitPrice = 80000m
            });

            products.Add(new Product
            {
                Id = 3,
                Name = "Thrift",
                Description = "Fashion accessories",
                UnitPrice = 23500m
            });

			products.Add(new Product
			{
				Id = 4,
				Name = "Earrings",
				Description = "Fashion accessories",
				UnitPrice = 2500m
			});

			products.Add(new Product
			{
				Id = 5,
				Name = "Pyjamas",
				Description = "Fashion accessories",
				UnitPrice = 125600m
			});

			products.Add(new Product
			{
				Id = 6,
				Name = "Hair Bonnet",
				Description = "Fashion accessories",
				UnitPrice = 888800m
			});
			return View(products);
        }
        public IActionResult ProductFromDb() 
        {

            var products =_dbContext.Products.Include(p=>p.Category).ToList();
            return View(products);

        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _dbContext.Categories.ToList();
            List<SelectListItem> categoriesList =categories.Select(c=>new SelectListItem
            {
                Value = c.Id.ToString(),
                Text= c.Name,
            }).ToList();
            return View(categoriesList);
        }
    }
}
