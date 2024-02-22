using CarRentalApp.BuisnessLayer.IServices;
using CarRentalApp.BuisnessLayer.Services;
using CarRentalApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CarRentalApp.APILayer.Controllers
{
    [Authorize(Roles ="customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService) 
        {
            _customerService = customerService; 
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterDTO customer)
        {
            try
            {
                return await _customerService.Register(customer) ? Ok("customer registered successfully") : BadRequest("Failed to register customer");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginCustomer(LoginDTO adminAuth)
        {
            try
            {
                string token = await _customerService.Login(adminAuth);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("AddVerificationDetails")]
        [HttpPut]
        public async Task<IActionResult> AddVerificationDetails(CustomerVerificationDTO customerVerificationDetails)
        {
            try
            {
                var res = await _customerService.AddVerificationDetails(customerVerificationDetails);

                if (res)
                    return Ok("Verification Details Added Successfully!");

                throw new Exception("Failed To add verification details!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }

        [AllowAnonymous]
        [Route("searchCar")]
        [HttpGet]
        public async Task<IActionResult> SearchCar([FromQuery] DateTime dateFrom, DateTime dateTo, string? brand=null, string? model=null, string? bodyType=null, string? fuelType = null)
        {
            try
            {
                var res = await _customerService.SearchCars(dateFrom, dateTo, brand, model, bodyType, fuelType);
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

        [Route("bookcar")]
        [HttpPost]
        public async Task<IActionResult> BookCar(ReservationsDTO reservation)
        {
            try
            {
               bool res = await _customerService.BookCar(reservation);
               if(res)
                    return Ok("Successfully booked!");

                throw new Exception("Failed to book!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        
        [Route("GetCustomerDetails/{email}")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetails(string email)
        {
            try
            {
                var details = await _customerService.GetCustomerDetails(email);
                return Ok(details);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            


        }
    }
}
