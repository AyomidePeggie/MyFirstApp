using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
	public class RegisterModel
	{
		public string Username { get; set; }
		[Required(ErrorMessage = "Password is Required")]
		[StringLength(50, ErrorMessage ="Password must be at least 6 characters", MinimumLength = 6)]
		[DataType(DataType.Password)]
		
		public string  Password { get; set; }
		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="InValid EmailAddress")]	
		public string Email { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password",ErrorMessage ="Password do not match")]
		public string	ConfirmPassword { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }


	}
}
