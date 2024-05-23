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
        private readonly IInstitutionRepository _institutionRepository;

        public ProjectController(IProjectRepository projectRepository, IInstitutionRepository institutionRepository)
        {
            _projectRepository = projectRepository;
            _institutionRepository = institutionRepository;
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

        [HttpPost]
        public async Task<JsonResult> PrepareProjectInstitution(int projectid)
        {

            var projectVM = await _projectRepository.GetProjectForUpsert(projectid);
            var projectInstitutionId = projectVM.InstitutionId;
            var institutions = await _institutionRepository.GetInstitutions();
            var selectListInstitutionsVm = institutions.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = projectVM != null && 
                projectVM.InstitutionId != 0 && 
                projectVM.InstitutionId != null ? 
                x.Id == projectInstitutionId : false
            });
            ViewBag.Institutions = selectListInstitutionsVm;

            return Json(selectListInstitutionsVm);
        }

        public async Task<ActionResult> GetUpsert(int id)
        {
            try
            {
                var updateProjectVm = await _projectRepository.GetProjectForUpsert(id);
                await PrepareProjectInstitution(id);
                return PartialView("~/Views/Project/Upsert.cshtml", updateProjectVm);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }


        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _projectRepository.Delete(id);
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