
namespace CarRentalApp.DataLayer.Entities
{
    public class Customer:AuditFields
    {
        public  int  CustId { get; set; }
        public string Name { get; set; }  = null!;
        public string Email { get; set; }  = null!;
        public string Phone { get; set; }  = null!;
        public string? DrivingLiscenceNumber {get; set; }
        public string? DL_img_front { get; set; }
        public string? DL_img_back { get; set; }

        public string? AddressIdType { get; set; }
        public string? AddressIdNumber { get; set; }

        public string VerifiedStatus { get; set; } = "not_verified";


        //navigation properties
        public IEnumerable<Reservation> Reservations { get; set; }  = null!;

    }
}
