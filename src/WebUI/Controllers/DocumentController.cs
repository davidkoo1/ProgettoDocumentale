using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.DTO;
using BLL.TableParameters;
using FluentValidation;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using BLL.DTO.DocumentDTOs;
using System.Collections.Generic;
using System.Linq;
using BLL.Common.Extensions;
using BLL.DTO.ProjectDTOs;
using BLL.Validator;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Operator Cedacri")]
    public class DocumentController : BaseController
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IValidator<CreateDocumentDto> _CreateDocumentDtoValidator;

        public DocumentController(IDocumentRepository documentRepository, IInstitutionRepository institutionRepository, IValidator<CreateDocumentDto> createDocumentDtoValidator)
        {
            _documentRepository = documentRepository;
            _institutionRepository = institutionRepository;
            _CreateDocumentDtoValidator = createDocumentDtoValidator;
        }

        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllDocumentsThree()
        {
            var result = await _documentRepository.GetAllThree();
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> LoadDocumentDatatable(DataTablesParameters parameters, string resource1, string resource2, string resource3)
        {
            try
            {
                var result = await _documentRepository.GetAllDocuments(parameters, resource1, resource2, resource3);
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

                var document = await _documentRepository.GetDocument(id);
                return PartialView("~/Views/Document/Details.cshtml", document);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }



        private async Task PrepareViewBags()
        {

            
            var institutions = await _institutionRepository.GetInstitutions();
            var selectListInstitutionsVm = institutions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = false
            });
            ViewBag.Institutions = selectListInstitutionsVm;

            var MacroTypes = await _documentRepository.GetMacroDocumentType();
            var selectListMacroTypeVm = MacroTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = false
            });
            ViewBag.MacroTypes = selectListMacroTypeVm;
        }

        [HttpPost]
        public async Task<JsonResult> GetMicroTypes(int MacroId)
        {
            var result = await _documentRepository.GetMicroTypesByMacroId(MacroId);
            var selectListMicroTypesVm = result.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = false
            });
            return Json(selectListMicroTypesVm);
        }

        [HttpPost]
        public async Task<JsonResult> GetProjectsByInstitution(int InstitutionId)
        {
            var result = await _documentRepository.GetProjectsByInstitutionId(InstitutionId);
            var selectListProjectsVm = result.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = false
            });
            return Json(selectListProjectsVm);
        }

        [HttpGet]
        public async Task<ActionResult> GetCreate()
        {
            try
            {

                await PrepareViewBags();
                return PartialView("~/Views/Document/Create.cshtml");
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }


        }



        [HttpPost]
        public async Task<ActionResult> Create(CreateDocumentDto createDocumentDto)
        {
            try
            {
                var validationResult = _CreateDocumentDtoValidator.Validate(createDocumentDto);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    await PrepareViewBags();
                    return PartialView("~/Views/Document/Create.cshtml", createDocumentDto);
                }

                //if (upsertProjectDto.Id == 0)
                //{
                //    upsertProjectDto.UserId = User.GetUserId();
                //}
                createDocumentDto.UserId = User.GetUserId();
                var result = await _documentRepository.CreateDocument(createDocumentDto);
                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorDocument"] = "ErrorCreateDocument";
                    await PrepareViewBags();
                    return PartialView("~/Views/Document/Create.cshtml");
                }



            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }

    }
}