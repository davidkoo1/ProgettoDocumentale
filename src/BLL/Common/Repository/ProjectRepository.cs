using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using BLL.TableParameters;
using DAL.Context.Persistance;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Save() => await _dbContext.SaveChangesAsync() > 0;

        public async Task<bool> Delete(int projectId)
        {
            var projectToDelete = await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
            projectToDelete.IsActive = !projectToDelete.IsActive;

            _dbContext.Projects.AddOrUpdate(projectToDelete);

            return await Save();  
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjects(DataTablesParameters parameters, string InstitutionId, string YearGroup)
        {
            IQueryable<Project> query = _dbContext.Projects
                 .Include(x => x.Institution)
                 .Include(x => x.User);
                 //.Search(parameters).OrderBy(parameters).Page(parameters);

            if (!string.IsNullOrEmpty(InstitutionId))
            {
                int IdForInstitution = Convert.ToInt32(InstitutionId);
                query = query.Where(x => x.Institution.Id == IdForInstitution);
            }

            if (!string.IsNullOrEmpty(YearGroup) && int.TryParse(YearGroup, out int year))
            {
                query = query.Where(x => x.DateTill.Year == year);
            }


            // Materialize query result
            return await query.Select(x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                DateFrom = x.DateFrom,
                DateTill = x.DateTill,
                InstitutionId = x.Institution.Name,
                UserId = x.User.Name,
                IsActive = x.IsActive,
            }).ToListAsync();
        }

        public async Task<IEnumerable<BuilderProjectThree>> GetAllThree()
        {
            var projects = await _dbContext.Projects
                .Include(i => i.Institution)
                .Include(u => u.User)
                .Include(d => d.Documents).ToListAsync();



            var grouped = projects
              .GroupBy(doc => doc.Institution)
              .Select(instGroup => new BuilderProjectThree
              {
                  InstitutionId = instGroup.Key.Id,
                  InstitutionName = instGroup.Key.Name,
                  YearGroups = instGroup
                      .Select(doc => doc.DateTill.Year) 
                      .Distinct() 
                      .ToList()
              }).ToList();

            return grouped;
        }

        public async Task<ProjectDetailDto> GetProject(int ProjectId)
        {
           return await _dbContext.Projects.Select(x => new ProjectDetailDto
           {
               Id = x.Id,
               Name = x.Name,
               InstitutionName = x.Institution.Name,
               DateFrom = x.DateFrom,
               DateTill = x.DateTill,
               AdditionalInfo = x.AdditionalInfo,
               IsActive= x.IsActive,

           }).FirstOrDefaultAsync(p => p.Id == ProjectId);
        }

        public async Task<UpsertProjectDto> GetProjectForUpsert(int ProjectId)
        {
            if (await _dbContext.Projects.AnyAsync(x => x.Id == ProjectId))
            {
                return await _dbContext.Projects
                            .Include(x => x.Institution)
                            .Select(t => new UpsertProjectDto
                            {
                                Id= t.Id,
                                Name= t.Name,
                                InstitutionId = t.Institution.Id,
                                DateFrom = t.DateFrom,
                                DateTill = t.DateTill,
                                AdditionalInfo = t.AdditionalInfo,
                                IsActive = t.IsActive,
                            })
                            .FirstOrDefaultAsync(x => x.Id == ProjectId);
            }
            return new UpsertProjectDto();
        }

        public async Task<bool> UpsertProject(UpsertProjectDto ProjectDtoToUpsert)
        {
            var existing = await _dbContext.Projects.AnyAsync(x => x.Id == ProjectDtoToUpsert.Id /*&& !x.IsActive*/);

            if (ProjectDtoToUpsert == null || !existing && ProjectDtoToUpsert.Id > 0)
            {
                return false;
            }

            if (ProjectDtoToUpsert.Id != 0)
            {

                var project = await _dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == ProjectDtoToUpsert.Id);
                ProjectDtoToUpsert.UserId = project.UserId;
            }

            _dbContext.Projects.AddOrUpdate(new Project
            {
                Id = ProjectDtoToUpsert.Id,
                Name = ProjectDtoToUpsert.Name,
                DateFrom = ProjectDtoToUpsert.DateFrom,
                DateTill = ProjectDtoToUpsert.DateTill,
                AdditionalInfo = ProjectDtoToUpsert.AdditionalInfo,
                IsActive = ProjectDtoToUpsert.IsActive,
                InstitutionId = ProjectDtoToUpsert.InstitutionId,
                UserId = ProjectDtoToUpsert.UserId
            });

            return await Save();
        }
    }
}
