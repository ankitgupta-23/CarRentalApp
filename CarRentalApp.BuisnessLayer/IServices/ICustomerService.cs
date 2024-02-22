using CarRentalApp.DTO;
using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.BuisnessLayer.IServices
{
	public interface ICustomerService
	{
		Task<bool> Register(CustomerRegisterDTO customer);
		
		Task<string> Login(LoginDTO customerAuth);

		/*Task<bool> UpdateDetails(int id);*/

		Task<bool> DeleteAccount(int id);

		Task<bool> AddVerificationDetails(CustomerVerificationDTO customerVerificationDTO);

		
		Task<IEnumerable<CarsDTO>> SearchCars(DateTime dateFrom, DateTime dateTo, string brand, string model, string bodyType, string fuelType);

		Task<CustomerDTO> GetCustomerDetails(string userEmail);
		Task<bool> BookCar(ReservationsDTO reservationsDTO);
	}
}
