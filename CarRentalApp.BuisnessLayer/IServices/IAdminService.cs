using CarRentalApp.DTO;

namespace CarRentalApp.BuisnessLayer.IServices
{
    public interface IAdminService
    {
        Task<bool> AddRole(string roleName);

        Task<bool> RegisterAdmin(string Email, string Password);

        Task<string> Login(LoginDTO adminAuth);

        Task<bool> VerifyCustomer(int customerId);

        Task<IEnumerable<CustomerDTO>> GetPendingVerfications();
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();

        Task<bool> AddCar(CarsDTO car);


    }
}
