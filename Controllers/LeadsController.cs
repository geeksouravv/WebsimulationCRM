using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsimulationCRM.CORE.DTO;

namespace WebsimulationCRM.UI.Controllers
{
    [Authorize]
    public class LeadsController : Controller
    {
        [Authorize]
        public IActionResult AddLead()
        {
            return View();
        }

        [Authorize]

        [HttpPost]
        public IActionResult AddLead(LeadAddRequest leadAddRequest)
        {
            return View();
        }

        [Authorize]

        public IActionResult GetLead()
        {
            return View();
        }
    }
}
