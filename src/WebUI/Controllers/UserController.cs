using BLL.Common.Interfaces;
using BLL.DTO;
using BLL.TableParameters;
using BLL.Validator;
using FluentValidation;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserDto> _CreateUserDtoValidator;
        private readonly IValidator<UpdateUserDto> _UpdateUserDtoValidator;
        public UserController(IUserRepository userRepository, 
            IValidator<CreateUserDto> CreateUserDtoValidator, 
            IValidator<UpdateUserDto> updateUserDtoValidator)
        {
            _userRepository = userRepository;
            _CreateUserDtoValidator = CreateUserDtoValidator;
            _UpdateUserDtoValidator = updateUserDtoValidator;
        }


        public ActionResult Index() => View();


        [HttpPost]
        public async Task<ActionResult> LoadDatatable(DataTablesParameters parameters)
        {
            try
            {
                var result = await _userRepository.GetAllUsers(parameters);
                return Json(new
                {
                    draw = parameters.Draw,
                    recordsFiltered = parameters.TotalCount,
                    recordsTotal = parameters.TotalCount,
                    data = result
                });

            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {

                var user = await _userRepository.GetUser(id);

                return PartialView("~/Views/User/Details.cshtml", user);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }

        private async Task PrepareUserRoles(UpdateUserDto userDto)
        {
            var rolesVm = await _userRepository.GetRolesAsync();
            var selectListItemRoleVm = rolesVm.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = userDto != null ? userDto.RolesId.Contains(x.Id) : false
            });

            ViewBag.Roles = selectListItemRoleVm;


        }

        public async Task<ActionResult> Create()
        {
            try
            {
                //var userVm = await _userRepository.GetUser(id);

                await PrepareUserRoles(null);
                return PartialView("~/Views/User/Create.cshtml");
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }


        }
        // POST: User/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateUserDto createUser)
        {
            try
            {
                var validationResult = _CreateUserDtoValidator.Validate(createUser);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    TempData["ErrorUser"] = "ErrorUser";
                    await PrepareUserRoles(null);
                    return PartialView("~/Views/User/Create.cshtml", createUser);
                }

                var result = await _userRepository.Add(createUser);
                if (result)
                {
                    // return Json(new { success = true/*, redirectUrl = Url.Action(nameof(Index))*/ });
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorUser"] = "ErrorUser";
                    await PrepareUserRoles(null);
                    return PartialView("~/Views/User/Create.cshtml", createUser);
                }

            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }


        public async Task<ActionResult> Edit(int id)
        {
            try
            {

                var user = await _userRepository.GetUpdateUser(id);
                await PrepareUserRoles(user);
                return PartialView("~/Views/User/Edit.cshtml", user);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Edit(int userId, UpdateUserDto updateUserDto)
        {
            var validationResult = _UpdateUserDtoValidator.Validate(updateUserDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                TempData["ErrorUser"] = "ErrorUser";
                await PrepareUserRoles(null);
                return PartialView("~/Views/User/Edit.cshtml", updateUserDto);
            }
            updateUserDto.Id = userId;
            var result = await _userRepository.Update(updateUserDto);
            if (result)
            {
                // return Json(new { success = true/*, redirectUrl = Url.Action(nameof(Index))*/ });
                return Json(new { success = true });
            }
            else
            {
                TempData["ErrorUser"] = "ErrorUser";
                await PrepareUserRoles(null);
                return PartialView("~/Views/User/Edit.cshtml", updateUserDto);
            }
        }
        //GET: User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {

                var user = await _userRepository.GetUser(id);

                return PartialView("~/Views/User/Delete.cshtml", user);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }


        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _userRepository.Delete(id);
                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }

    }
}