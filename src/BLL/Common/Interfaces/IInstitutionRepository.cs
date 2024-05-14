using BLL.DTO;
using BLL.DTO.InstitutionDTOs;
using BLL.TableParameters;
using BLL.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IInstitutionRepository
    {

        Task<IEnumerable<InstitutionDto>> GetAllInstitutions(DataTablesParameters parameters);
        Task<IEnumerable<InstitutionDto>> GetInstitutions();
        Task<InstitutionDto> GetInstitution(int id);
        Task<UpdateInstitutionDto> GetInstitutionForUpdate(int id);
        Task<bool> Add(CreateInstitutionDto institutionToCreateDto);
        Task<bool> Update(UpdateInstitutionDto institutionToUpdateDto);
        Task<bool> Delete(int institutionId);
        Task<bool> Save();
    }
}
