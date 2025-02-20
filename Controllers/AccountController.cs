using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Entities.Identity;
using MyFirstApp.Models;

namespace MyFirstApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel loginmodel)
		{
			var user = await _userManager.FindByNameAsync(loginmodel.Username);
			if (user == null) 
			return View(loginmodel);

			//try to sign the user in with the credentials they supplied

			var result = await _signInManager.PasswordSignInAsync(loginmodel.Username
				,loginmodel.Password, false,true);
			if (result.Succeeded)
			{
				return View(loginmodel);
			}
			else
			{
				ModelState.AddModelError("", "Invalid username or password");
				return View(loginmodel);
			}
		}
	}

	
}
