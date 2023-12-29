using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    public class PricingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
