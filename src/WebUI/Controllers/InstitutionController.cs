using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.DTO.InstitutionDTOs;
using BLL.TableParameters;
using BLL.UserDTOs;
using BLL.Validator;
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
    public class InstitutionController : BaseController
    {

        private readonly IInstitutionRepository _institutionRepository;

        private readonly IValidator<CreateInstitutionDto> _CreateInstitutionDtoValidator;
        private readonly IValidator<UpdateInstitutionDto> _UpdateInstitutionDtoValidator;
        public InstitutionController(IInstitutionRepository institutionRepository, 
            IValidator<CreateInstitutionDto> createInstitutionDtoValidator,
            IValidator<UpdateInstitutionDto> updateInstitutionDtoValidator)
        {
            _institutionRepository = institutionRepository;
            _CreateInstitutionDtoValidator = createInstitutionDtoValidator;
            _UpdateInstitutionDtoValidator = updateInstitutionDtoValidator;
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


        [HttpPost]
        public async Task<JsonResult> GetInstitutions(int? InstitutionId)
        {
            var institutions = await _institutionRepository.GetInstitutions();
            var selectListinstitutionsVm = institutions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == InstitutionId
            });
            ViewBag.Institutions = selectListinstitutionsVm;
            return Json(selectListinstitutionsVm);



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
        public ActionResult GetCreate() => PartialView("~/Views/Institution/Create.cshtml");

        [HttpPost]
        public async Task<ActionResult> Create(CreateInstitutionDto createInstitution)
        {
            try
            {
                var validationResult = _CreateInstitutionDtoValidator.Validate(createInstitution);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    TempData["ErrorInstitution"] = "ErrorInstitution";
                    return PartialView("~/Views/Institution/Create.cshtml", createInstitution);
                }

                var result = await _institutionRepository.Add(createInstitution);
                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorInstitution"] = "ErrorInstitution";
                    return PartialView("~/Views/Institution/Create.cshtml", createInstitution);
                }

            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }


        [HttpGet]
        public async Task<ActionResult> GetEdit(int id)
        {
            try
            {

                var institution = await _institutionRepository.GetInstitutionForUpdate(id);
                return PartialView("~/Views/Institution/Edit.cshtml", institution);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }
        [HttpPost]
        public async Task<ActionResult> Edit(int institutionId, UpdateInstitutionDto updateInstitutionDto)
        {
            var validationResult = _UpdateInstitutionDtoValidator.Validate(updateInstitutionDto);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                TempData["ErrorInstitution"] = "ErrorInstitution";
                return PartialView("~/Views/Institution/Edit.cshtml", await _institutionRepository.GetInstitutionForUpdate(institutionId));
            }
            updateInstitutionDto.Id = institutionId;
            var result = await _institutionRepository.Update(updateInstitutionDto);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                TempData["ErrorInstitution"] = "ErrorInstitution";
                return PartialView("~/Views/Institution/Edit.cshtml", await _institutionRepository.GetInstitutionForUpdate(institutionId));
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetDelete(int id)
        {
            try
            {

                //var institution = await _institutionRepository.GetInstitution(id);

                return PartialView("~/Views/Institution/Delete.cshtml", id);
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
                var success = await _institutionRepository.Delete(id);
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