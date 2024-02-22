using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.DTO
{
	public class CarsDTO
	{
        public int Id { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string BodyType { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public int NumberOfSeats { get; set; }
        public string CarImagePath { get; set; }

        public int RegistrationNumber { get; set; }
    }
}
