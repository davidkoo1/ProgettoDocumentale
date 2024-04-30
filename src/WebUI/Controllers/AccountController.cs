using BLL.Common.Interfaces;
using BLL.DTO;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountInterface _accountInterface;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
            
        public AccountController(IAccountInterface accountInterface)
        {
            _accountInterface = accountInterface;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            //var response = new LoginDto();
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto loginVM)
        {
            //return View();
            try
            {
                if (!ModelState.IsValid)
                    return View();
                var userVm = await _accountInterface.GetUserForLogin(loginVM);

                if (userVm == null || !userVm.IsEnabled)
                {
                    TempData["ErrorAccount"] = "SomethingError";
                    return View(loginVM);
                }
                else
                {
                    ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    var userClaims = new List<Claim>
                     {
                         new Claim(ClaimTypes.NameIdentifier, userVm.Id.ToString()),
                         new Claim(ClaimTypes.Name, userVm.UserName),
                         new Claim(ClaimTypes.Email, userVm.Email),
                         //new Claim(ClaimTypes.Role, userVm.)

                     };

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View(loginVM);
            }
        }
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("SignIn", "Account");
            //return RedirectToAction("SignIn");
        }
    }
}