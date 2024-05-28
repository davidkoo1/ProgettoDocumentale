using BLL.Common.Repository;
using BLL.DTO.DocumentDTOs;
using BLL.DTO.InstitutionDTOs;
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
        Task<IEnumerable<ProjectDto>> GetProjectsByInstitutionId(int? InstitutionId);
        Task<bool> CreateDocument(CreateDocumentDto createDocumentDtoFromForm);
        Task<UpdateDocumentDto> GetUpdateDocument(int id);
        Task<bool> Update(UpdateDocumentDto updateDocumentDto);
        Task<bool> Save();
    }
}
