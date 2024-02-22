using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DTO
{
	public class ReservationsDTO
	{
		public int ReservationId { get; set; }
		public int CustomerId { get; set; }
		public int CarId { get; set; }
		public int RegistrationNumber { get; set; }

		public DateTime ReservationFrom { get; set; }
		public DateTime ReservationTo { get; set; }

		public int TotalHours { get; set; }

		public string ReservationStatus { get; set; }
	}
}
