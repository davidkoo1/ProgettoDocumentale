using AutoMapper;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using DAL.Context.Persistance;
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
