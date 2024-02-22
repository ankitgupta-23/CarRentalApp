using CarRentalApp.BuisnessLayer.IServices;
using CarRentalApp.BuisnessLayer.Services;
using CarRentalApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CarRentalApp.APILayer.Controllers
{
    [Authorize(Roles ="admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
       
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [Route("addrole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            { 
                return await _adminService.AddRole(roleName) ? Ok("Role Created Successfully") : BadRequest("Failed to create role");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(string Email, string Password)
        {
            try
            {
                return await _adminService.RegisterAdmin(Email, Password) ? Ok("Admin registered successfully") : BadRequest("Failed to register admin");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginDTO adminAuth)
        {
            try
            {
                var user  =  await _adminService.Login(adminAuth);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("VerifyCustomer")]
        [HttpPut]
        public async Task<IActionResult> VerifyCustomer(int customerId)
        {
            try
            {
                bool res = await _adminService.VerifyCustomer(customerId);
                return Ok("Customer Verified Successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("GetVerificationPendingList")]
        [HttpGet]
        public async Task<IActionResult> GetPendingList()
        {
            try
            {
                var res = await _adminService.GetPendingVerfications();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetAllCustomer")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {

            try
            {
                var res = await _adminService.GetAllCustomers();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        [Route("AddCar")]
        [HttpPost]
        public async Task<IActionResult> AddCar(CarsDTO car)
        {
            try
            {
                var res = await _adminService.AddCar(car);
                if(res)
                    return Ok("Car Added Successfully");

                throw new Exception("Failed to add car");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message + "\n" + ex.InnerException);
            }

            
            
        }


    }
}
