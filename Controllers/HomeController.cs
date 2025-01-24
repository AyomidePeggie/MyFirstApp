using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Models;
using System.Diagnostics;

namespace MyFirstApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string Name = "Jayden";
            ViewBag.ForeignName = Name;
            ViewData["Ayomide"] = "one million";
            int Newnumber = 3 * 7;
            return View(Newnumber);
        }

        public IActionResult Privacy()
        {
            List<string> Products = new List<string>();
            Products.Add("Toothpaste");
            Products.Add("Noodles");
            Products.Add("Biscuit");
            Products.Add("Gloves");
            Products.Add("Sweet");
            Products.Add("Candy");
            Products.Add("Milk");
            Products.Add("Gum");
            return View(Products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
