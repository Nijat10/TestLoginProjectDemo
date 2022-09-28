using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using TestLogin.Models;
using TestLogin.ViewModels;

namespace TestLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UserName, string Password)
        {
            LoginRequest request = new LoginRequest();
            request.UserName = UserName;
            request.Password = Password;
            using (ICUTechClient client = new ICUTechClient())
            {
                string loginResponse = client.Login(request.UserName, request.Password, "");
                ResponseVM response = JsonConvert.DeserializeObject<ResponseVM>(loginResponse);
                if (response.ResultCode == -1)
                {
                    TempData["ErrorMessage"] = "Login Failed: Your user name or password is incorrect";
                    return View();
                }
                else
                {
                    TempData["SuccessMessage"] = "Login successfull ! You have successfully signed into your account";
                    TempData["Username"] = UserName;

                    return View();
                }
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Email, string Password,
            string FirstName, string LastName, string Mobile, int CountryID,
            int aID, string SignupIP)
        {
            using (ICUTechClient client = new ICUTechClient())
            {
                var response = client.RegisterNewCustomer(Email, Password,
                    FirstName, LastName,
                    Mobile, CountryID, aID, SignupIP);

                return View();
            }
        }
    }
}
