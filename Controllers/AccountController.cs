using Microsoft.AspNetCore.Mvc;

namespace WebsimulationCRM.Controllers
{
    public class AccountController : Controller
    {
        [Route("/")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
