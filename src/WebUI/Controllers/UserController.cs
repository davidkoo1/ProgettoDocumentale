using BLL.Common.Interfaces;
using BLL.TableParameters;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository) => _userRepository = userRepository;
        

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
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var success = _userRepository.Delete(id);
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