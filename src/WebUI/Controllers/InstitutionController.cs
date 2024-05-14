using BLL.Common.Interfaces;
using BLL.TableParameters;
using BLL.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class InstitutionController : Controller
    {

        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionController(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }


        [HttpPost]
        public async Task<ActionResult> LoadInstitutionDatatable(DataTablesParameters parameters)
        {
            try
            {
                var result = await _institutionRepository.GetAllInstitutions(parameters);
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

        [HttpGet]
        public async Task<ActionResult> GetDetails(int id)
        {
            try
            {

                var user = await _institutionRepository.GetInstitution(id);

                return PartialView("~/Views/Institution/Details.cshtml", user);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }


        [HttpGet]
        public ActionResult GetCreate() =>  PartialView("~/Views/Institution/Create.cshtml");


        

    }
}