﻿using BLL.Common.Interfaces;
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
        private readonly IValidator<UpdateDocumentDto> _UpdateDocumentDtoValidator;

        public DocumentController(IDocumentRepository documentRepository, IInstitutionRepository institutionRepository,
            IValidator<CreateDocumentDto> createDocumentDtoValidator, IValidator<UpdateDocumentDto> updateDocumentDtoValidator)
        {
            _documentRepository = documentRepository;
            _institutionRepository = institutionRepository;
            _CreateDocumentDtoValidator = createDocumentDtoValidator;
            _UpdateDocumentDtoValidator = updateDocumentDtoValidator;
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

        private async Task ViewBagForEdit(int? DocumentInstitutionId,
            int DocumentMacroId,
            int? DocumentMicroId,
            int? DocumentProjectId)
        {

            var institutions = await _institutionRepository.GetInstitutions();
            var selectListInstitutionsVm = institutions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = DocumentInstitutionId != 0 ? x.Id == DocumentInstitutionId : false
            });
            ViewBag.Institutions = selectListInstitutionsVm;


            var MacroTypes = await _documentRepository.GetMacroDocumentType();
            var selectListMacroTypeVm = MacroTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = DocumentMacroId != 0 ? x.Id == DocumentMacroId : false
            });
            ViewBag.MacroTypes = selectListMacroTypeVm;


            var MicroTypes = await _documentRepository.GetMicroTypesByMacroId(DocumentMacroId);
            var selectListMicroTypeVm = MicroTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = DocumentMicroId != 0 ? x.Id == DocumentMicroId : false
            });
            ViewBag.MicroTypes = selectListMicroTypeVm;


            var projects = await _documentRepository.GetProjectsByInstitutionId(DocumentInstitutionId);
            var selectListProjectsVm = projects.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = DocumentMacroId != 3 ? x.Id == DocumentProjectId : false
            });
            ViewBag.Projects = selectListProjectsVm;
        }
        [HttpGet]
        public async Task<ActionResult> GetEdit(int id)
        {
            try
            {
                var updateDocumentVm = await _documentRepository.GetUpdateDocument(id);
                await ViewBagForEdit(updateDocumentVm.InstitutionId,
                    updateDocumentVm.MacroId,
                    updateDocumentVm.MicroId,
                    updateDocumentVm.ProjectId);

                return PartialView("~/Views/Document/Edit.cshtml", updateDocumentVm);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }


        }


        [HttpPost]
        public async Task<ActionResult> Edit(int documentId, UpdateDocumentDto updateDocumentDto)
        {
            try
            {
                updateDocumentDto.Id = documentId;
                var validationResult = _UpdateDocumentDtoValidator.Validate(updateDocumentDto);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                    }

                    await ViewBagForEdit(updateDocumentDto.InstitutionId,
                    updateDocumentDto.MacroId,
                    updateDocumentDto.MicroId,
                    updateDocumentDto.ProjectId);
                    return PartialView("~/Views/Document/Edit.cshtml", updateDocumentDto);
                }


                var result = await _documentRepository.Update(updateDocumentDto);
                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    TempData["ErrorDocument"] = "ErrorEditDocument";
                    await ViewBagForEdit(updateDocumentDto.InstitutionId,
                    updateDocumentDto.MacroId,
                    updateDocumentDto.MicroId,
                    updateDocumentDto.ProjectId);
                    return PartialView("~/Views/Document/Edit.cshtml", updateDocumentDto);
                }



            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }

    }
}