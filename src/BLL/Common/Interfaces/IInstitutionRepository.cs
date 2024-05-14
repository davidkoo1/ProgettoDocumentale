using BLL.DTO;
using BLL.DTO.InstitutionDTOs;
using BLL.TableParameters;
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
        Task<InstitutionDto> GetInstitution(int id);
    }
}
