using CustomerManagement.Data;
using CustomerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login user)
        {
            if (user.Username == "admin" && user.Password == "admin")
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Customer");
            }

            ViewBag.Message = "Invalid login credentials.";
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    

     [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
    }
}
