using Microsoft.AspNetCore.Mvc;
using SMS_MVC.Models;
using System.Diagnostics;

namespace SMS_MVC.Controllers
{
    public class AccountsController : Controller
    {
        private SMSDBContext _Context;
        public AccountsController(SMSDBContext context)
        {
            _Context = context;
        }

        public IActionResult Login()
        {
            return View("Login", new LoginModel());
        }

        [HttpPost]
        public IActionResult OnLoginClicked(LoginModel model)
        {
            var IsValidUser = _Context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (IsValidUser != null)
            {
                return RedirectToAction("Index", "Users");
            }
            model.ErrorMessage = "Invalid email or password.";
            return View("Login", model);
        }

        public IActionResult OnLogoutClicked()
        {
            return RedirectToAction("Login");
        }

    }
}
