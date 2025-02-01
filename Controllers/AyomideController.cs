using Microsoft.AspNetCore.Mvc;

namespace MyFirstApp.Controllers
{
    public class AyomideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
