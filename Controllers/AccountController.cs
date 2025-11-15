using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsimulationCRM.CORE.Domain.IdentityEntities;
using WebsimulationCRM.CORE.DTO;
using WebsimulationCRM.CORE.Enums;

namespace WebsimulationCRM.UI.Controllers
{

    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
           if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

           var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(loginDTO);
            }

            var result =  await _signInManager.PasswordSignInAsync(user, loginDTO.Password, isPersistent: false,lockoutOnFailure: false);

            if(result.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == "EmployeeName"))
                {
                    await _userManager.AddClaimAsync(user, new Claim("EmployeeName", user.EmployeeName));
                }

                await _signInManager.RefreshSignInAsync(user);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Login", "Invalid Email or Password");
            return View(loginDTO);
        }

        [AllowAnonymous]
 
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp =>  temp.Errors).Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                EmployeeName = registerDTO.UserName,
                Role = registerDTO.Role.ToString(),
                TeamName = registerDTO.TeamName
            };

           Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(user,registerDTO.Password);
            if(result.Succeeded)
            {
                //create role
                if(registerDTO.Role == UserTypeOptions.SuperAdmin)
                {
                    if(await _roleManager.FindByNameAsync(UserTypeOptions.SuperAdmin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.SuperAdmin.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    //add role
                   await _userManager.AddToRoleAsync(user,UserTypeOptions.SuperAdmin.ToString());
                }
                else if(registerDTO.Role == UserTypeOptions.Manager)
                {
                    if(await _roleManager.FindByNameAsync(UserTypeOptions.Manager.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() {Name = UserTypeOptions.Manager.ToString() };
                       await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Manager.ToString());
                }

                else
                {
                    if(await _roleManager.FindByNameAsync(UserTypeOptions.Employee.ToString()) is null)
                     {
                        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Employee.ToString() };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Employee.ToString());
                }

                await _userManager.AddClaimAsync(user, new Claim("EmployeeName", user.EmployeeName));

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerDTO);
            }
        
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //public async Task<IActionResult> IsEmailAlreadyRegistered(string Email)
        //{
        //   ApplicationUser user = await _userManager.FindByEmailAsync(email);

        //    if(user == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }

        //}
    }
}
