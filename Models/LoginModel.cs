using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
	public class LoginModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
