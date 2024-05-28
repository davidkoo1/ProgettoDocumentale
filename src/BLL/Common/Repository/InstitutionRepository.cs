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
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

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

        public async Task<bool> Add(CreateInstitutionDto institutionToCreateDto)
        {
            var existing = await _dbContext.Institutions.AnyAsync(x => x.InstCode == institutionToCreateDto.InstCode);

            if (institutionToCreateDto == null || existing == true)
            {
                return false;
            }
            var institutionToCreate = _mapper.Map<Institution>(institutionToCreateDto);
            _dbContext.Institutions.Add(institutionToCreate);
            return await Save();
        }

        public async Task<bool> Delete(int institutionId)
        {
            var institutionToDelete = _dbContext.Institutions.Include(x => x.Projects).Include(u => u.Users).FirstOrDefault(x => x.Id == institutionId);
            //userToDelete.IsEnabled = false;
            _dbContext.Institutions.Remove(institutionToDelete);
            return await Save();
        }

        public async Task<IEnumerable<InstitutionDto>> GetAllInstitutions(DataTablesParameters parameters) => _mapper.Map<IEnumerable<InstitutionDto>>(
            await _dbContext.Institutions
            .Search(parameters)
            .OrderBy(parameters)
            .Page(parameters)
            .ToListAsync());

        public async Task<IEnumerable<InstitutionDto>> GetInstitutions() => _mapper.Map<IEnumerable<InstitutionDto>>(await _dbContext.Institutions.ToListAsync());

        public async Task<InstitutionDto> GetInstitution(int id) => _mapper.Map<InstitutionDto>(await _dbContext.Institutions.FirstOrDefaultAsync(u => u.Id == id));

        public async Task<bool> Save() => await _dbContext.SaveChangesAsync() > 0 ? true : false;

        public async Task<bool> Update(UpdateInstitutionDto institutionToUpdateDto)
        {
            var institution = _dbContext.Institutions.AsNoTracking().FirstOrDefault(x => x.Id == institutionToUpdateDto.Id);
            if (institution == null)
                return false;
            var institutionToUpdate = _mapper.Map<Institution>(institutionToUpdateDto);
            institutionToUpdate.InstCode = institution.InstCode;

            _dbContext.Institutions.AddOrUpdate(institutionToUpdate);

            return await Save();
        }

        public async Task<UpdateInstitutionDto> GetInstitutionForUpdate(int id) => _mapper.Map<UpdateInstitutionDto>(await _dbContext.Institutions.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<bool> IsCodeInUseAsync(string code) => await _dbContext.Institutions.AnyAsync(u => u.InstCode.Equals(code));
    }
}
