
namespace CarRentalApp.DataLayer.Entities
{
    public class Reservation:AuditFields
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public int RegistrationNumber { get; set; }

        public DateTime ReservationFrom { get; set; }
        public DateTime ReservationTo { get; set; }

        public int TotalHours { get; set;  }

        public string ReservationStatus { get; set; }

        public int PaymentId { get; set; }


        //navigation properties

        public Customer Customer { get; set; }

        public Car Car { get; set; }

        public TotalCar TotalCar { get; set; }

        public Payment Payment { get; set; }
    }
}
