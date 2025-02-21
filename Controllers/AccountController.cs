using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFirstApp.Entities.Identity;
using MyFirstApp.Models;
using System.Security.Claims;

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
				var firstNameClaim =new Claim("Firstname", user.FirstName);
				var lastNameClaim = new Claim ("Lastname", user.LastName);
				var dateOfBirthClaim = new Claim(ClaimTypes.DateOfBirth, "2001-10-01");

				await _userManager.AddClaimAsync(user, firstNameClaim);
				await _userManager.AddClaimAsync(user, lastNameClaim);
				await _userManager.AddClaimAsync(user, dateOfBirthClaim);

				await _signInManager.RefreshSignInAsync(user);

				var roles = await _userManager.GetRolesAsync(user);

				if (roles.Any(x => x == "Admin"))
				{
					return Redirect(Url.Action("Index", "Admin"));
				}
				else if (roles.Any(x => x == "Visitor"))
				{
					return Redirect(Url.Action("Index", "Home"));
				}
				return Redirect(GetRedirectUrl("/"));

			
			}
			else
			{
				ModelState.AddModelError("", "Invalid username or password");
				return View(loginmodel);
			}
		}

		private string GetRedirectUrl(string returnUrl)
		{
			if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
			{
				return Url.Action("Index", "Home");
			}
			return returnUrl;
		}
	}

	
}
