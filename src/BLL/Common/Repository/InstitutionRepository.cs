using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.InstitutionDTOs;
using BLL.TableParameters;
using BLL.UserDTOs;
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
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IMapper _mapper;

        public InstitutionRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<InstitutionDto>> GetAllInstitutions(DataTablesParameters parameters) => _mapper.Map<IEnumerable<InstitutionDto>>(
            await _dbContext.Institutions
            .Search(parameters)
            .OrderBy(parameters)
            .Page(parameters)
            .ToListAsync());

        public async Task<InstitutionDto> GetInstitution(int id) => _mapper.Map<InstitutionDto>(await _dbContext.Institutions.FirstOrDefaultAsync(u => u.Id == id));
    }
}
