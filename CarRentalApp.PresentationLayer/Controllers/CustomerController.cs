using CarRentalApp.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace CarRentalApp.PresentationLayer.Controllers
{
    [Authorize(Roles="customer")]
    public class CustomerController : Controller
    {
        
        private const string baseURL =  "http://localhost:5034/api/";
        HttpClient httpClient;
        
        private readonly IWebHostEnvironment _hostEnv;

        public CustomerController(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
        }

        public IActionResult Index()
        {   
            

            var email = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var token = HttpContext.User.FindFirst("token")?.Value;
            
            if(email!=null && token !=null)
            {
                using(HttpClient client = new HttpClient()){
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    
                    var res = client.GetAsync("customer/GetCustomerDetails/"+email).Result;
                    if(res.IsSuccessStatusCode)
                    {
                        var content = res.Content.ReadAsStringAsync().Result;
                        var customerInfo = JsonConvert.DeserializeObject<CustomerDTO>(content);
                        
                        return View(customerInfo);
                    }

                    HttpContext.SignOutAsync();
                    TempData["login-error"] = "Failed to Authenticate! Login Again";
                    return View("Login");
                }
            }

            HttpContext.SignOutAsync();
            TempData["login-error"] = "Failed to Authenticate! Login Again";
            return View("Login");
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginDTO customerLoginCredentials)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(customerLoginCredentials);
                var jsonData = new StringContent(serializedData, UnicodeEncoding.UTF8, "application/json");

                var response = httpClient.PostAsync("customer/login", jsonData).Result;

                if(response.IsSuccessStatusCode)
                {
                    string token = response.Content.ReadAsStringAsync().Result;
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, customerLoginCredentials.Email),
                        new Claim(ClaimTypes.Role, "customer"),
                        new Claim("token", token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    
                    return RedirectToAction("Index");  
                }

                TempData["login-error"] = response.Content.ReadAsStringAsync().Result;

                return View(customerLoginCredentials);
            }
            catch(HttpRequestException httpEx)
            {
                TempData["login-error"] = "Failed to reach the server! try again after some time";
                return View();
            }
            catch(AggregateException aggreEx){
                TempData["login-error"] = "Failed to reach the server! try again after some time";
                return View();
            }
           
            
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() 
        {
            return View();  
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(CustomerRegisterDTO customerRegistration)
        {
            try{

                var serializedData = JsonConvert.SerializeObject(customerRegistration);
                var jsonData = new StringContent(serializedData, UnicodeEncoding.UTF8, "application/json");

                var response = httpClient.PostAsync("customer/register", jsonData).Result;
               
                if (response.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["registration-error"] = response.Content.ReadAsStringAsync().Result;
                    return View();
                }
            }
            catch(HttpRequestException httpexc)
            {
                TempData["registration-error"] = "Failed to reach the server! try again after some time";
                return View();
            }
            catch(AggregateException aggreEx){
                TempData["registration-error"] = "Failed to reach the server! try again after some time";
                return View();
            }
        }


        [HttpGet]
        public IActionResult SearchCar()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SearchCarResults(DateTime dateFrom, DateTime dateTo, string? brand = null, string? model = null, string? bodyType = null, string? fuelType = null)
        {   

            return View();
        }

       
        [Route("Customer/AddVerificationData")]
        [HttpPost]
        public JsonResult AddVerificationData(IFormCollection data)
        {
            if(data==null)
                return Json("data was null");
            

            

            CustomerVerificationDTO customerVerificationDTO = new CustomerVerificationDTO()
            {
                CustId = int.Parse(data["CustId"]),
                DrivingLiscenceNumber = data["DrivingLiscenceNumber"],
                AddressIdNumber = data["AddressIdNumber"],
                AddressIdType = data["AddressIdType"]
            };
            
            var email = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var token = HttpContext.User.FindFirst("token")?.Value;
           

            var files = data.Files;
            Console.WriteLine(files.Count);

            IFormFile img_front_DL = files["img_front_DL"];
            IFormFile img_back_DL = files["img_back_DL"];

            //Console.WriteLine(files.Count);
            if(img_front_DL!=null && img_back_DL!=null && img_front_DL.Length>0 && img_front_DL.Length>1)
            {
                string wwwRootPath = _hostEnv.WebRootPath;

                string front_pathName = "DLImage_front_" + Guid.NewGuid() + Path.GetExtension(img_front_DL.FileName);
                string front_savepath = Path.Combine(wwwRootPath, "userDLImages", front_pathName);
                
                string back_pathName = "DLImage_back_" + Guid.NewGuid() + Path.GetExtension(img_back_DL.FileName);
                string back_savepath = Path.Combine(wwwRootPath, "userDLImages", back_pathName);
                

                using (var fileStream = new FileStream(front_savepath, FileMode.Create))
                {
                    {
                        img_front_DL.CopyToAsync(fileStream);
                    }
                }

                using (var fileStream = new FileStream(back_savepath, FileMode.Create))
                {
                    {
                        img_back_DL.CopyToAsync(fileStream);
                    }
                }

                customerVerificationDTO.DL_img_front = front_pathName;
                customerVerificationDTO.DL_img_back = back_pathName;

            }
            else
            {
                return Json("Upload Both Images before Submit");
            }
            
            if(email!=null && token !=null)
            {

                using(HttpClient client = new HttpClient()){
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var serializedData = JsonConvert.SerializeObject(customerVerificationDTO);
                    var jsonData = new StringContent(serializedData, UnicodeEncoding.UTF8, "application/json");
                    var res = client.PutAsync("Customer/AddVerificationDetails", jsonData).Result;
                    if(res.IsSuccessStatusCode)

                        return Json("successfully updated");

                    return Json("Could not add verificaiton Details 1");
                }
                
            }

            return Json("Could not add verificaiton Details 2");
        }
    }
}
