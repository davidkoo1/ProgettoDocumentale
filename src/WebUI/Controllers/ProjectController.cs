using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.Common.Repository;
using BLL.DTO.ProjectDTOs;
using BLL.TableParameters;
using BLL.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        private async Task PrepareProjectInstitution(int projectid)
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
        public async Task<ActionResult> Upsert(UpsertProjectDto upsertProjectDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upsertProjectDto.Id == 0)
                    {
                        upsertProjectDto.UserId = User.GetUserId();
                    }
                    var result = await _projectRepository.UpsertProject(upsertProjectDto);
                    if (result)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        TempData["ErrorProject"] = "ErrorUpsertProject";
                        await PrepareProjectInstitution(upsertProjectDto.Id);
                        return PartialView("~/Views/Project/Upsert.cshtml", upsertProjectDto);
                    }

                }
                TempData["ErrorProject"] = "ErrorUpsertProject";
                await PrepareProjectInstitution(upsertProjectDto.Id);
                return PartialView("~/Views/Project/Upsert.cshtml", upsertProjectDto);
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