using CarRentalApp.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;


namespace CarRentalApp.PresentationLayer.Controllers{
    public class AdminController:Controller{
        public IActionResult Index(){
            return View();
        }
    }
}