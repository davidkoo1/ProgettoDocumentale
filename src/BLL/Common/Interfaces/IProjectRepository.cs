using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using BLL.TableParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<BuilderProjectThree>> GetAllThree();
        Task<IEnumerable<ProjectDto>> GetAllProjects(DataTablesParameters parameters, string InstitutionId, string YearGroup);
        Task<ProjectDetailDto> GetProject(int ProjectId);
        Task<UpsertProjectDto> GetProjectForUpsert(int ProjectId);
        Task<bool> Delete(int projectId);
        Task<bool> Save();
    }
}
