using Microsoft.AspNetCore.Mvc;

namespace PackPal.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(){  //controllers can return: view(usually.cshtml),redirect,JSON result,file or nothing
            return View(); //since it returns it will look for view called Login
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
