using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.TableParameters;
using DAL.Context.Persistance;
using DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Common.Repository
{


    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public DocumentRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocuments(DataTablesParameters parameters, string resource1, string resource2, string resource3)
        {
            List<Document> documents;
            documents = await _dbContext.Documents
               .Include(x => x.Institution)
               .Include(x => x.DocumentType)
               .Search(parameters).OrderBy(parameters).Page(parameters)
               .ToListAsync();
            if (!string.IsNullOrEmpty(resource1))
            {
                documents = documents.Where(x => x.Institution.Name == resource1).ToList();
            }
            if (!string.IsNullOrEmpty(resource2))
            {
                documents = documents.Where(x => x.Institution.Name == resource1 && x.GroupingDate.Year == int.Parse(resource2)).ToList();
            }
            if (!string.IsNullOrEmpty(resource3))
            {
                var idDocument = await _dbContext.DocumentTypes.FirstOrDefaultAsync(x => x.Name == resource3);
                var idDocument2 = await _dbContext.DocumentTypeHierarchies
    .Where(x => x.IdMacro == idDocument.Id)
    .Select(x => x.IdMicro) // Извлекаем только IdMicro
    .ToListAsync();

                documents = documents.Where(x =>
     x.Institution.Name == resource1 &&
     x.GroupingDate.Year == int.Parse(resource2) &&
     idDocument2.Contains(x.TypeId)) // Проверяем, содержится ли TypeId в списке
     .ToList();

            }

            return documents.Select(x => new DocumentDto
            {
                Id = x.Id,
                Name = x.Name,
                SavedPath = x.SavedPath,
                UploadDate = x.UploadDate,
                AdditionalInfo = x.AdditionalInfo,
                GroupingDate = x.GroupingDate,
                InstitutionId = x.Institution.Name,
                TypeId = x.DocumentType.Name,
            });
        }

        public async Task<IEnumerable<BuilderDocumentThree>> GetAllThree()
        {
            var documents = await _dbContext.Documents
                   .Include(doc => doc.Institution)
                   .Include(doc => doc.DocumentType)
                   .ToListAsync();

            var documentTypes = _dbContext.DocumentTypes.ToList();
            var typeHierarchy = _dbContext.DocumentTypeHierarchies.ToList();

            var grouped = documents.GroupBy(doc => doc.Institution)
                            .Select(instGroup => new BuilderDocumentThree
                            {
                                InstitutionName = instGroup.Key.Name,
                                YearGroups = instGroup.GroupBy(doc => doc.GroupingDate.Year)
                                .Select(yearGroup => new YearDocumentTypeGroup
                                {
                                    Year = yearGroup.Key,
                                    // Сбор имен макро-типов для документов в каждом году
                                    SubTypeNames = yearGroup
                                        .SelectMany(doc => typeHierarchy.Where(x => x.IdMicro == doc.TypeId).Select(x => x.IdMacro)) // Извлекаем IdMacro
                                        .Distinct()
                                        .Join(documentTypes, idMacro => idMacro, dt => dt.Id, (idMacro, dt) => dt.Name) // Присоединяем DocumentTypes для получения имени
                                        .OrderBy(name => name)
                                        .ToList(),
                                }).OrderBy(y => y.Year).ToList()
                            }).ToList();



            return grouped;
        }

    }
}
