using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DataLayer.Entities
{
    public class TotalCar:AuditFields
    {
        public int RegistrationNumber { get; set; }

        public int CarId { get; set; }

        //navigation properties
        public Car Car { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
