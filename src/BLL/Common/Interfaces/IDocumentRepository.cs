using BLL.DTO.DocumentDTOs;
using BLL.DTO.InstitutionDTOs;
using BLL.TableParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IDocumentRepository
    {
        //dynamic TestMethod();
        Task<IEnumerable<DocumentItem>> GetAllDocuments(DataTablesParameters parameters);
        Task<IEnumerable<DocumentViewModel>> GetAllThree();
        ////Task<IEnumerable<InstitutionDto>> GetInstitutions();
        ////Task<InstitutionDto> GetInstitution(int id);
        ////Task<UpdateInstitutionDto> GetInstitutionForUpdate(int id);
        //Task<bool> Add(CreateDocumentDto documentToCreateDto);
        ////Task<bool> Update(UpdateInstitutionDto institutionToUpdateDto);
        //Task<bool> Delete(int documentId);
        //Task<bool> Save();
    }
}
