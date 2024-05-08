using BLL.Common.Interfaces;
using BLL.DTO;
using FluentValidation;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountInterface;
        private readonly IValidator<LoginDto> _LoginDtoValidator;
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public AccountController(IAccountRepository accountInterface, IValidator<LoginDto> LoginDtoValidator)
        {
            _accountInterface = accountInterface;
            _LoginDtoValidator = LoginDtoValidator;
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
            try
            {
                var validationResult = _LoginDtoValidator.Validate(loginVM);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    return View(loginVM);
                }

                loginVM.Username = "Crme145";
                loginVM.Password = "Cedacri1234567!";
                //return View();
                var userVm = await _accountInterface.GetUserForLogin(loginVM);

                if (userVm == null || !userVm.IsEnabled)
                {
                    TempData["ErrorAccount"] = "SomethingError";
                    return View();
                }
                else
                {
                    ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, userVm.Id.ToString(), ClaimValueTypes.String));
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userVm.Email, ClaimValueTypes.String));
                    claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                        "OWIN Provider", ClaimValueTypes.String));
                    if (userVm.UserRoles != null)
                        foreach (var userRole in userVm.UserRoles)
                            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole, ClaimValueTypes.String));

                    //if (userVm.UserRole != null)
                    //{
                    //    claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, userVm.UserRole, ClaimValueTypes.String));
                    //}
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
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}