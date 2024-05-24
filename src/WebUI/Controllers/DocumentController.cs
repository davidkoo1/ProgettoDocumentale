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

namespace WebUI.Controllers
{
    [Authorize(Roles = "Operator Cedacri")]
    public class DocumentController : BaseController
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
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

    }
}