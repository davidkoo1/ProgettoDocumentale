using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.Common.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Operator Bancar")]
    public class BankController : Controller
    {
        private readonly IBankRepository _bankRepository;
        private readonly IDocumentRepository _documentRepository;
        public BankController(IBankRepository bankRepository, IDocumentRepository documentRepository)
        {
            _bankRepository = bankRepository;
            _documentRepository = documentRepository;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> GetServiceDocuments()
        {
            try
            {

                var resultService = await _bankRepository.GetAllSerciceThree(User.GetUserId());
                return PartialView("~/Views/Bank/_ServiceReports.cshtml", resultService);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetSLADocuments()
        {
            try
            {

                var resultSLA = await _bankRepository.GetAllSLAThree(User.GetUserId());
                return PartialView("~/Views/Bank/_SLAReports.cshtml", resultSLA);
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
                return PartialView("~/Views/Bank/_Info.cshtml", document);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }

        [HttpGet]
        public async Task<ActionResult> DownloadDocumentById(int id)
        {
            try
            {
                var result = await _bankRepository.GetFileById(id);

                if (result != null && System.IO.File.Exists(result.Path))
                {
                    var streamResponse = new FileStream(result.Path, FileMode.Open, FileAccess.Read);

                    return File(streamResponse, "application/octet-stream", result.Name);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}