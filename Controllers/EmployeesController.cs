
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsimulationCRM.CORE.Domain.IdentityEntities;
using WebsimulationCRM.CORE.DTO;
using WebsimulationCRM.CORE.ServiceContracts;
using WebsimulationCRM.Infrastructure.DBContext;

namespace WebsimulationCRM.UI.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly INotificationService _notify;
        public EmployeesController(INotificationService notificationService)
        {
            _notify = notificationService;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult AllEmployees()
        {
          var employees =  _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.EmployeeName,
                    u.Email,
                    u.Role,
                    u.TeamName,
                    u.PhoneNumber

                }).ToList();

            return View(employees);
        }

        //public async Task<IActionResult> Edit(string Id)
        //{
        //    if(Id == null)
        //    {
        //        _notify.Error("Invalid User");
        //        return NotFound();
        //    }

        //    _userManager.Users.Where(temp => temp.Id == Id.ToString().Select(e => new EditEmployeeDTO
        //    {
        //        Id = e.Id,


        //    });
        //}
    }
}
