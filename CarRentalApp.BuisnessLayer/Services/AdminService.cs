using AutoMapper;
using CarRentalApp.BuisnessLayer.IServices;
using CarRentalApp.DataLayer.Entities;
using CarRentalApp.DataLayer.IRepository;
using CarRentalApp.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Reflection.Metadata;
using System.Security.Claims;


namespace CarRentalApp.BuisnessLayer.Services
{
    public class AdminService : IAdminService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        private readonly IConfiguration _config;

        private readonly IGenericRepository<Car> _carRepository;
        private readonly IGenericRepository<TotalCar> _carTotalsRepository;

        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration config, IGenericRepository<Customer> customerRepository, IMapper mapper, IGenericRepository<Car> carRepository, IGenericRepository<TotalCar> carTotalsRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _carRepository = carRepository;
            _carTotalsRepository = carTotalsRepository;
        }

        public async Task<bool> AddRole(string roleName)
        {
            var res = await _roleManager.CreateAsync(new IdentityRole(roleName));

            return res.Succeeded;
            
        }

        public async Task<string> Login(LoginDTO adminAuth)
        {
            var adminUser = await _userManager.FindByEmailAsync(adminAuth.Email);
            
            if (adminUser!=null)
            {
                bool isCorrectCredentials = await _userManager.CheckPasswordAsync(adminUser, adminAuth.Password);
                if(isCorrectCredentials)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, adminUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, "admin")
                    };

                    return JWTToken.GetJWTToken(_config, authClaims);
                }


                throw new Exception("Password is not correct!");
            }


            throw new Exception("Email is Not Found!");
        }

        public async Task<bool> RegisterAdmin(string Email, string Password)
        {
            
            var checkExistance = await _userManager.FindByNameAsync(Email);
            if(checkExistance == null)
            {
                IdentityUser adminUser = new IdentityUser() { Email = Email, UserName = Email };
                var res = await _userManager.CreateAsync(adminUser, Password);

                if (res.Succeeded)
                    return _userManager.AddToRoleAsync(adminUser, "admin").Result.Succeeded;

                string errors = string.Empty;
                foreach(var er in res.Errors)
                {
                    errors += er.Description + "\n";
                }

                throw new Exception(errors);
            }

            throw new Exception("Email already Exists!");
        }

        public async Task<bool> VerifyCustomer(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new Exception("Customer doesn't Exists, Verification failed!");

            customer.VerifiedStatus = "verified";

            bool res = await _customerRepository.UpdateAsync(customer);

            return res ? res: throw new Exception("Verification Failed!");

        }

        public async Task<IEnumerable<CustomerDTO>> GetPendingVerfications()
        {
            var res = await _customerRepository.GetAll().Where(x=>x.VerifiedStatus!="verified").ToListAsync();
            return _mapper.Map<List<CustomerDTO>>(res);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var res = await _customerRepository.GetAll().ToListAsync();
            return _mapper.Map<List<CustomerDTO>>(res);
        }

        public async Task<bool> AddCar(CarsDTO car)
        {
           
            var carExsits = _carRepository.GetAll().Where(x=>x.Brand==car.Brand && x.Model == car.Model && x.BodyType == car.BodyType).FirstOrDefault();
            
            if (carExsits==null)
            {
                var toInsert = _mapper.Map<Car>(car);
                var res = await _carRepository.AddAsync(toInsert);
                if (res)
                {
                    bool isRegistrationExits = _carTotalsRepository.GetAll().Any(x=>x.RegistrationNumber == car.RegistrationNumber);
                    if (!isRegistrationExits)
                    {
                        var carInstance = new TotalCar { CarId = toInsert.Id, RegistrationNumber = car.RegistrationNumber };
                        var res2 = await _carTotalsRepository.AddAsync(carInstance);
                        return res2;
                    }

                    throw new Exception($"The registration number  {car.RegistrationNumber} already exists!");

                }

                throw new Exception("Failed to add car!");
            }
            else
            {
                var res3 = await _carTotalsRepository.AddAsync(new TotalCar { CarId = carExsits.Id, RegistrationNumber = car.RegistrationNumber });
                return res3;
            }
           
        }
    }
}
