using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreCRUD.Models
{
	public class User
	{
		[Key]
		public int userId { get; set; }

		[Required(ErrorMessage = "Username is requried!")]
		public string username { get; set; }

		[Required(ErrorMessage = "Password is requried!")]
		public string password { get; set; }
		public string role { get; set; }
	}
}
