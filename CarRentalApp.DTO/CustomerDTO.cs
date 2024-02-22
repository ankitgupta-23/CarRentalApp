
namespace CarRentalApp.DTO
{
	public class CustomerDTO
	{
		public int CustId { get; set; }
		public string Name { get; set; }  = null!;
		public string Email { get; set; }  = null!;
		public string Phone { get; set; }  = null!;
		public string DrivingLiscenceNumber { get; set; }  = null!;
		public string DL_img_front { get; set; }  = null!;
		public string DL_img_back { get; set; }  = null!;

		public string AddressIdType { get; set; }  = null!;
		public string AddressIdNumber { get; set; }  = null!;

		public string VerifiedStatus { get; set; }  = null!;

		public string Password { get; set; }  = null!;

		public IEnumerable<ReservationsDTO> Reservations { get; set; }  = null!;
	}
}
