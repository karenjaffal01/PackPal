using Microsoft.AspNetCore.Mvc;

namespace PackPal.Controllers
{
    public class TripController : Controller
    {
        public IActionResult TripView()
        {
            return View();
        }
    }
}
