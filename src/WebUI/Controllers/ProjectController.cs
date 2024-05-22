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
        public async Task<ActionResult> LoadProjectDatatable(DataTablesParameters parameters, string resource1, string resource2)
        {
            try
            {
                var result = await _projectRepository.GetAllProjects(parameters, resource1, resource2);
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