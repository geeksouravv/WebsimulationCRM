using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebsimulationCRM.CORE.Domain.IdentityEntities;
using WebsimulationCRM.CORE.DTO;

namespace WebsimulationCRM.UI.Controllers
{

    public class AccountController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                Role = registerDTO.Role,
                TeamName = registerDTO.TeamName
            };

           Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(user,registerDTO.Password);
            if(result.Succeeded)
            {
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
