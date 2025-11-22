
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using WebsimulationCRM.CORE.Domain.IdentityEntities;
using WebsimulationCRM.CORE.DTO;
using WebsimulationCRM.CORE.ServiceContracts;
using WebsimulationCRM.Infrastructure.DBContext;

namespace WebsimulationCRM.UI.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly INotificationService _notify;

        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeesController(UserManager<ApplicationUser> userManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _notify = notificationService;
        }

        public IActionResult AllEmployees()
        {
            var employees = _userManager.Users
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

        public IActionResult EmployeesPDF()
        {
            var employees = _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.EmployeeName,
                    u.Email,
                    u.Role,
                    u.TeamName,
                    u.PhoneNumber

                }).ToList();

            return new ViewAsPdf("EmployeesPDF", employees, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
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
