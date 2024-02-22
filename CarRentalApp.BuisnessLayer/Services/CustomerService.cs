using AutoMapper;
using CarRentalApp.BuisnessLayer.IServices;
using CarRentalApp.DataLayer.Entities;
using CarRentalApp.DataLayer.IRepository;
using CarRentalApp.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace CarRentalApp.BuisnessLayer.Services
{                                            
    public class CustomerService : ICustomerService
    {    
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Car> _carRepository;
        private readonly IGenericRepository<Reservation> _reservationRepository;
        private readonly IGenericRepository<TotalCar> _carTotalsRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public CustomerService(IGenericRepository<Customer> customerRepository, IMapper mapper, UserManager<IdentityUser> userManager, IConfiguration config, IGenericRepository<Car> carRepository, IGenericRepository<Reservation> reservationRepository, IGenericRepository<TotalCar> carTotalsRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
            _carRepository = carRepository;
            _reservationRepository = reservationRepository;
            _carTotalsRepository = carTotalsRepository;
        }

        public async Task<bool> AddVerificationDetails(CustomerVerificationDTO customerVerificationDTO)
        {
            var res = await _customerRepository.GetByIdAsync(customerVerificationDTO.CustId);
            if (res != null)
            {
                res.AddressIdNumber = customerVerificationDTO.AddressIdNumber;
                res.DrivingLiscenceNumber = customerVerificationDTO.DrivingLiscenceNumber;
                res.DL_img_back = customerVerificationDTO.DL_img_back;
                res.DL_img_front = customerVerificationDTO?.DL_img_front;
                res.AddressIdType = customerVerificationDTO?.AddressIdType;
                res.VerifiedStatus = "pending";
                
                bool isUpdated = await _customerRepository.UpdateAsync(res);

                return isUpdated;
            }

            throw new Exception("Customer Details not found!");

        }



        public async Task<bool> DeleteAccount(int id)
        {

            Customer customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                bool res = await _customerRepository.DeleteAsync(_mapper.Map<Customer>(customer));

                return res ? res : throw new Exception("Failed to Delete User!");
            }

            throw new Exception("User Not found to delete!");

        }


        public async Task<string> Login(LoginDTO customerAuth)
        {
            var customerUser = await _userManager.FindByEmailAsync(customerAuth.Email);

            if (customerUser != null)
            {
                bool isCorrectCredentials = await _userManager.CheckPasswordAsync(customerUser, customerAuth.Password);
                if (isCorrectCredentials)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, customerUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, "customer")
                    };

                    return JWTToken.GetJWTToken(_config, authClaims);
                }


                throw new Exception("Password is not correct!");
            }


            throw new Exception("Email is Not Found!");
        }

        public async Task<bool> Register(CustomerRegisterDTO customer)
        {

            var user = await _userManager.FindByNameAsync(customer.Email);
            if (user == null)
            {

                IdentityUser identityUser = new IdentityUser()
                {
                    UserName = customer.Email,
                    Email = customer.Email,
                    PhoneNumber = customer.Phone,

                };


                var isAdded = await _userManager.CreateAsync(identityUser, customer.Password);
                if (isAdded.Succeeded)
                {
                    bool result = await _customerRepository.AddAsync(_mapper.Map<Customer>(customer));
                    if (result)
                    {
                        var isRoleAssigned = await _userManager.AddToRoleAsync(identityUser, "customer");
                        return isRoleAssigned.Succeeded;
                    }

                    throw new Exception("Failed to register customer");

                }

                string errors = string.Empty;
                foreach (var er in isAdded.Errors)
                {
                    errors += er.Description + "\n";
                }

                throw new Exception(errors);
            }

            throw new Exception("User already Exists with same Email!");

        }

        public async Task<CustomerDTO> GetCustomerDetails(string userEmail)
        {
            var res = await _customerRepository.GetAll().Where(x=>x.Email.ToLower()==userEmail.ToLower()).FirstOrDefaultAsync();

            if (res != null)
            {
                return _mapper.Map<CustomerDTO>(res);
            }

            throw new Exception("Customer Not Found!");
        }

        public async Task<IEnumerable<CarsDTO>> SearchCars(DateTime dateFrom, DateTime dateTo, string brand, string model, string bodyType, string fuelType)
        {
            // var res = new List<CarsDTO>();



            var data = from cars in _carRepository.GetAll()
                       join cartotal in _carTotalsRepository.GetAll() on cars.Id equals cartotal.CarId
                       where !_reservationRepository.GetAll().Any(r => r.RegistrationNumber == cartotal.RegistrationNumber && (r.ReservationFrom < dateTo && r.ReservationTo > dateFrom))
                       select new CarsDTO
                       {
                          Id = cars.Id,
                           Brand = cars.Brand,
                           Model = cars.Model,
                           BodyType = cars.BodyType,
                           FuelType = cars.FuelType,
                           Description = cars.Description,
                           NumberOfSeats = cars.NumberOfSeats,
                           CarImagePath = cars.CarImagePath,
                           RegistrationNumber = cartotal.RegistrationNumber,

                       };


            



            if (!string.IsNullOrEmpty(brand))
            {
                data = data.Where(car => car.Brand == brand);
            }

            if (!string.IsNullOrEmpty(model))
            {
                data = data.Where(car => car.Model.ToLower() == model.ToLower());
            }

            if (!string.IsNullOrEmpty(bodyType))
            {
                List<string> bodyTypeList = new List<string>(bodyType.ToLower().Split(','));
                data = data.Where(car => bodyTypeList.Contains(car.BodyType.ToLower()));
            }

            if (!string.IsNullOrEmpty(fuelType))
            {
                List<string> fuelTypeList = new List<string>(fuelType.ToLower().Split(','));
                data = data.Where(car => fuelTypeList.Contains(car.FuelType.ToLower()));
            }

            var res = await data.ToArrayAsync();


            return _mapper.Map<List<CarsDTO>>(res);


           
        }

        public async Task<bool> BookCar(ReservationsDTO reservationsDTO)
        {
            bool res = _customerRepository.GetByIdAsync(reservationsDTO.CustomerId).Result.VerifiedStatus == "verified";
            if (res)
            {
                bool isBooked = await _reservationRepository.AddAsync(_mapper.Map<Reservation>(reservationsDTO));
                return isBooked;
            }

            throw new Exception("You are not a verified user! You cannot book any car until you get verified!");
            
            
        }
    }


}
