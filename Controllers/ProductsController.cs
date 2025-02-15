using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstApp.Data;
using MyFirstApp.Entities;
using MyFirstApp.Models;

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

            var products = _dbContext.Products.Include(p => p.Category).ToList();
            return View(products);

        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _dbContext.Categories.ToList();
            List<SelectListItem> categoriesList = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            var productModel = new ProductModel();
            productModel.Categories = categoriesList;
            return View(productModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductModel model ) 
        {
            //create a list to store the file paths associated with the given product 
            List<string> ImagePaths = new();

            //handle the uploaded images 

            if (model.Images!= null && model.Images.Count>0)
            {
                foreach(var imagefile in model.Images) 
                {
                    if (imagefile!=null && imagefile.Length > 0) 
                    {
                        //let us save the image to a location e.g a folder on a server
                        // let us also generate a unique file name  to avoid any name clashes 
                        var filename = Guid.NewGuid() + Path.GetExtension(imagefile.FileName);
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", filename);
                        using (var stream = new FileStream(filepath,FileMode.Create))
                        {
                            await imagefile.CopyToAsync(stream);
                        }
                        //store the filepath 
                        ImagePaths.Add("images/"+ filename);
                    }
                }
            }
            
            var product = new Product 
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,
                Category =_dbContext.Categories.Find(model.SelectedCategoryId)
            };
            _dbContext.Products.Add(product);
           await _dbContext.SaveChangesAsync();

            //associate the uploaded images filepath with the product 
            if (ImagePaths.Count > 0)
            {
                foreach(var imagepath in ImagePaths)
                {
                    var image = new ProductImage
                    {
                        ProductId= product.Id,
                        ImagePath= imagepath
                    };
                    _dbContext.ProductImages.Add(image);

                }
               await _dbContext.SaveChangesAsync();
            }
			return Json(Url.Action("ProductFromDb", "Products"));

		}
        [HttpGet]
        public IActionResult EditProduct(int Id)
        {
            var product = _dbContext.Products.Include(p=> p.Category).FirstOrDefault(p=> p.Id==Id);
            if (product == null) 
            {
                return RedirectToAction("ProductFromDb");

            }
            var productmodel = new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Id = product.Id,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                SelectedCategoryId = product.Category == null ? 0 : product.Category.Id,
                Categories = _dbContext.Categories.ToList().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                }).ToList()
            };
            return View(productmodel);

        }
        [HttpPost]
        public IActionResult EditProduct(ProductModel model)
        {
            var product =_dbContext.Products.Find(model.Id);
            if (product == null) 
            {
                return RedirectToAction("ProductFromDb");
            }
            var category = _dbContext.Categories.Find(model.SelectedCategoryId);
            if ( category== null)
            {
                ModelState.AddModelError("SelectedCategoryId", "The category you selected does not exist");
                return View(model);
            }
            //let's update
            product.Name = model.Name;
            product.Description = model.Description;
            product.Quantity = model.Quantity;
            product.UnitPrice = model.UnitPrice;
            product.Category = category;
            _dbContext.SaveChanges();
            return RedirectToAction("ProductFromDb");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
			var product = _dbContext.Products.Find(id);
			if (product == null)
			{
				return RedirectToAction("ProductFromDb");
			}
            //remove the product from database
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return Json(Url.Action("ProductFromDb","Products"));
		}
        [HttpGet]
        public IActionResult GetDetails(int id)
        {
			var product = _dbContext.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
			if (product == null)
			{
				return RedirectToAction("ProductFromDb");

			}
            var imagePaths =_dbContext.ProductImages
                .Where(P=>P.ProductId == id)
                .Select(i=>i.ImagePath)
                .ToList();
			var productmodel = new ProductModel
			{
				Name = product.Name,
				Description = product.Description,
				Id = product.Id,
				Quantity = product.Quantity,
				UnitPrice = product.UnitPrice,
                ImagePaths = imagePaths,
				SelectedCategoryId = product.Category == null ? 0 : product.Category.Id,
				Categories = _dbContext.Categories.ToList().Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.Name,
				}).ToList()
			};
            var CategoryName =_dbContext.Categories.FirstOrDefault (c => c. Id == productmodel.SelectedCategoryId)?.Name;
            ViewBag.CategoryName = CategoryName;
			return View(productmodel);
		}
    }
}
