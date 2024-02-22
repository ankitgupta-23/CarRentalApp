

namespace CarRentalApp.DataLayer.Entities
{
    public class Car:AuditFields
    {
        public int Id { get; set; }
        

        public string Brand { get; set; }
        public string Model { get; set; }
        public string BodyType { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public int NumberOfSeats { get; set; }

        public string CarImagePath { get; set; }
        //navigation properties
       
        public IEnumerable<TotalCar> TotalCars { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }

    }
}
