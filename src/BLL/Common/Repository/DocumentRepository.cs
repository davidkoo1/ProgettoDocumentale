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
    public class InstitutionDocumentTypeGroup
    {
        public string InstitutionName { get; set; }
        public List<YearDocumentTypeGroup> YearGroups { get; set; }
    }

    public class YearDocumentTypeGroup
    {
        public int Year { get; set; }
        public List<string> SubTypeNames { get; set; }
    }

    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public DocumentRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocuments(DataTablesParameters parameters)
        {
            var documents = await _dbContext.Documents
                .Include(x => x.Institution)
                .Include(x => x.DocumentType)
                .Search(parameters).OrderBy(parameters).Page(parameters)
                .ToListAsync();

            return documents.Select(x => new DocumentDto
            {
                Id = x.Id,
                Name = x.Name,
                SavedPath = x.SavedPath,
                UploadDate = x.UploadDate,
                AdditionalInfo = x.AdditionalInfo,
                GroupingDate = x.GroupingDate,
                Institution = x.Institution.Name,
                DocumentType = x.DocumentType.Name,
            });
        }

        public async Task<IEnumerable<InstitutionDocumentTypeGroup>> GetAllThree()
        {
            var documents = await _dbContext.Documents
                   .Include(doc => doc.Institution)
                   .Include(doc => doc.DocumentType)
                   .ToListAsync();

            var documentTypes = _dbContext.DocumentTypes.ToList();
            var typeHierarchy = _dbContext.DocumentTypeHierarchies.ToList();

            var grouped = documents.GroupBy(doc => doc.Institution)
                            .Select(instGroup => new InstitutionDocumentTypeGroup
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
