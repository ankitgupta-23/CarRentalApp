using AutoMapper;
using CarRentalApp.DataLayer.Entities;
using CarRentalApp.DTO;


namespace CarRentalApp.BuisnessLayer.AutoMapperProfiles
{
	public class AutomapperProfile:Profile
	{
        public AutomapperProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerRegisterDTO>().ReverseMap();
            CreateMap<Car, CarsDTO>().ReverseMap();
            CreateMap<Reservation, ReservationsDTO>().ReverseMap();
            
        }
    }
}
