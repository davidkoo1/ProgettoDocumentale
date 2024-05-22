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
        private readonly IMapper _mapper;
        public ProjectRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjects(DataTablesParameters parameters, string resource1, string resource2)
        {
            IQueryable<Project> query = _dbContext.Projects
                 .Include(x => x.Institution)
                 .Include(x => x.User)
                 .Search(parameters).OrderBy(parameters).Page(parameters);

            if (!string.IsNullOrEmpty(resource1))
            {
                query = query.Where(x => x.Institution.Name == resource1);
            }

            if (!string.IsNullOrEmpty(resource2) && int.TryParse(resource2, out int year))
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
    }
}
