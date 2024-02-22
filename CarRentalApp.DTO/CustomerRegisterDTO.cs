using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.DTO
{
    public  class CustomerRegisterDTO
    {
        [Required]
        public string Name { get; set; }  = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Enter valid email address!")]
        public string Email { get; set; }  = null!;

        [Required]
        [RegularExpression("[6-9][0-9]{9}", ErrorMessage = "Enter valid phone number!")]
        public string Phone { get; set; }  = null!;

        [Required]
        public string Password { get;  set; }  = null!;

        [DisplayName("Confirm Password")]
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm password must match!")]
        public string ConfirmPassword { get; set; }  = null!;


    }
}
