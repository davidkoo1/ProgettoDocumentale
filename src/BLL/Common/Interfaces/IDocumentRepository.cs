using BLL.Common.Repository;
using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using BLL.TableParameters;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<DocumentDto>> GetAllDocuments(DataTablesParameters parameters, string resource1, string resource2, string resource3);
        Task<IEnumerable<BuilderDocumentThree>> GetAllThree();
        Task<DocumentDetailDto> GetDocument(int Id);
        Task<IEnumerable<DocumentTypeDto>> GetMacroDocumentType();
        Task<IEnumerable<DocumentTypeDto>> GetMicroTypesByMacroId(int MacroId);
        Task<IEnumerable<ProjectDto>> GetProjectsByInstitutionId(int InstitutionId);
        Task<bool> CreateDocument(CreateDocumentDto createDocumentDtoFromForm);
        ////Task<IEnumerable<InstitutionDto>> GetInstitutions();
        ////Task<InstitutionDto> GetInstitution(int id);
        ////Task<UpdateInstitutionDto> GetInstitutionForUpdate(int id);
        //Task<bool> Add(CreateDocumentDto documentToCreateDto);
        ////Task<bool> Update(UpdateInstitutionDto institutionToUpdateDto);
        //Task<bool> Delete(int documentId);
        Task<bool> Save();
    }
}
