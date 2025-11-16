using Microsoft.AspNetCore.Mvc;
using SMS_MVC.Models;

namespace SMS_MVC.Controllers
{
    public class AccountsController : Controller
    {
        public LoginModel loginModel = new LoginModel();
        public IActionResult Login()
        {
            return View("Login", loginModel);
        }

        [HttpPost]
        public IActionResult OnLoginClicked(LoginModel model)
        {
            return RedirectToAction("Index", "Users");
        }

        public IActionResult OnLogoutClicked()
        {
            return RedirectToAction("Login");
        }

    }
}
