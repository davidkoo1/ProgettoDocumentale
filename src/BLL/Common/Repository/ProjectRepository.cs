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
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<ProjectDto>> GetAllProjects(DataTablesParameters parameters, string InstitutionId, string YearGroup)
        {
            IQueryable<Project> query = _dbContext.Projects
                 .Include(x => x.Institution)
                 .Include(x => x.User)
                 .Search(parameters).OrderBy(parameters).Page(parameters);

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
    }
}
