using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsimulationCRM.CORE.DTO;
using WebsimulationCRM.CORE.ServiceContracts;

namespace WebsimulationCRM.UI.Controllers
{
    [Authorize]
    public class LeadsController : Controller
    {
        private readonly ILeadsAddService _leadsAddService;
        private readonly ILeadsGetService _leadsGetService;
        public LeadsController(ILeadsAddService leadAddService, ILeadsGetService leadsGetService)
        {
            _leadsAddService = leadAddService;
            _leadsGetService = leadsGetService;
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

        public async Task<IActionResult> AllLead()
        {
           var Leads = await _leadsGetService.GetAllLeads();
            return View(Leads);
        }

        [Authorize]
        public async Task<IActionResult> HotLeads()
        {
            var hotLeads = await _leadsGetService.GetHotLeads();
            return View(hotLeads);
        }

        [Authorize]
        public async Task<IActionResult> NewLeads()
        {
            var newLeads = await _leadsGetService.GetNewLeads();
            return View(newLeads);
        }

        [Authorize]
        public async Task<IActionResult> DeadLeads()
        {
            var deadLeads = await _leadsGetService.GetDeadLeads();
            return View(deadLeads);
        }

        [Authorize]
        public async Task<IActionResult> ColdLeads()
        {
            var coldLeads = await _leadsGetService.GetColdLeads();
            return View(coldLeads);
        }

        [Authorize]
        public async Task<IActionResult> WarmLeads()
        {
            var warmLeads = await _leadsGetService.GetWarmLeads();
            return View(warmLeads);
        }
    }
}
