using BLL.Common.Interfaces;
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

    }
}