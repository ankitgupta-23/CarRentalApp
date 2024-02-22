using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DTO
{
	public class LoginDTO
	{
		[Required]
		[EmailAddress(ErrorMessage = "Enter valid Email Address!")]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
	}
}
