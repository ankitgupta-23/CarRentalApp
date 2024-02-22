
using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.DTO
{
    public class CustomerVerificationDTO
    {
        public int CustId { get; set; }

        [Required]
        public string DrivingLiscenceNumber { get; set; } = null!;

        
        public string DL_img_front { get; set; }  = null!;

     
        public string DL_img_back { get; set; }  = null!;

        [Required]
        public string AddressIdType { get; set; }  = null!;

        [Required]
        public string AddressIdNumber { get; set; }  = null!;

    }
}
