﻿using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.DTO;
using BLL.TableParameters;
using FluentValidation;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;

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
        // GET: Document
        public async Task<ActionResult> Index()
        {
            var tmp = await _documentRepository.GetAllThree();
            return View(tmp);
        }

        [HttpPost]
        public async Task<ActionResult> LoadDocumentDatatable(DataTablesParameters parameters)
        {
            try
            {
                var result = await _documentRepository.GetAllDocuments(parameters);
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

    }
}