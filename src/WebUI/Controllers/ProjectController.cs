using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.TableParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpPost]
        public async Task<ActionResult> GetAllProjectsThree()
        {
            var result = await _projectRepository.GetAllThree();
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> LoadProjectDatatable(DataTablesParameters parameters, string InstitutionId, string YearGroup)
        {
            try
            {
                var result = await _projectRepository.GetAllProjects(parameters, InstitutionId, YearGroup);
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

                var project = await _projectRepository.GetProject(id);
                return PartialView("~/Views/Project/Details.cshtml", project);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }

        }

    }
}