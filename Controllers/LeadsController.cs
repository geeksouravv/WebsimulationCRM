using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsimulationCRM.CORE.DTO;
using WebsimulationCRM.CORE.ServiceContracts;

namespace WebsimulationCRM.UI.Controllers
{
    [Authorize]
    public class LeadsController : Controller
    {
        private readonly ILeadsAddService _leadsAddService;
        public LeadsController(ILeadsAddService leadAddService)
        {
            _leadsAddService = leadAddService;
        }


        [Authorize]
        public async Task< IActionResult> AddLead()
        {
          return View();
        }

        [Authorize]

        [HttpPost]
        public async Task<IActionResult> AddLeadAsync(LeadAddRequest leadAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(leadAddRequest);
            }

           await _leadsAddService.AddLeads(leadAddRequest);
            
            return RedirectToAction("Index","Home");

           
        }

        [Authorize]

        public IActionResult GetLead()
        {
            return View();
        }
    }
}
