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
using System.Xml.Linq;

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
            IQueryable<Document> query = _dbContext.Documents
                .Include(x => x.Institution)
                .Include(x => x.DocumentType)
                .Search(parameters).OrderBy(parameters).Page(parameters);

            if (!string.IsNullOrEmpty(resource1))
            {
                query = query.Where(x => x.Institution.Name == resource1);
            }

            if (!string.IsNullOrEmpty(resource2) && int.TryParse(resource2, out int year))
            {
                query = query.Where(x => x.GroupingDate.Year == year);
            }

            if (!string.IsNullOrEmpty(resource3))
            {
                var documentTypeId = await _dbContext.DocumentTypes
                    .Where(x => x.Name == resource3)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (documentTypeId != 0)
                {
                    var documentTypeIds = await _dbContext.DocumentTypeHierarchies
                        .Where(x => x.IdMacro == documentTypeId)
                        .Select(x => x.IdMicro)
                        .ToListAsync();

                    query = query.Where(x => documentTypeIds.Contains(x.DocumentType.Id));
                }
            }

            // Materialize query result
            var documents = await query.ToListAsync();

            // Process each document to create DocumentDto
            var documentDtos = new List<DocumentDto>();
            foreach (var x in documents)
            {
                var typeName = await GetFullDocumentTypeName(x.DocumentType);
                documentDtos.Add(new DocumentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SavedPath = x.SavedPath,
                    UploadDate = x.UploadDate,
                    AdditionalInfo = x.AdditionalInfo,
                    GroupingDate = x.GroupingDate,
                    InstitutionId = x.Institution.Name,
                    TypeId = typeName
                });
            }

            return documentDtos;
        }

        private async Task<string> GetFullDocumentTypeName(DocumentType documentType)
        {
            var hierarchy = await _dbContext.DocumentTypeHierarchies
                                            .FirstOrDefaultAsync(x => x.IdMicro == documentType.Id);

            var hierarchyName = hierarchy != null ?
                await _dbContext.DocumentTypes.FirstOrDefaultAsync(x => x.Id == hierarchy.IdMacro) : null;

            return hierarchyName != null ? $"{documentType.Name}/{hierarchyName.Name}" : documentType.Name;
        }

        public async Task<IEnumerable<BuilderDocumentThree>> GetAllThree()
        {
            var documents = await _dbContext.Documents
                 .Include(doc => doc.Institution)
                 .Include(doc => doc.DocumentType)
                 .ToListAsync();

            var typeHierarchy = await _dbContext.DocumentTypeHierarchies.ToListAsync();
            var documentTypes = await _dbContext.DocumentTypes.ToListAsync();


            var grouped = (from doc in documents
                           group doc by doc.Institution into instGroup
                           select new BuilderDocumentThree
                           {
                               InstitutionId = instGroup.Key.Id,
                               InstitutionName = instGroup.Key.Name,
                               YearGroups = (from doc in instGroup
                                             group doc by doc.GroupingDate.Year into yearGroup
                                             let macroIds = (from doc in yearGroup
                                                             from th in typeHierarchy
                                                             where th.IdMicro == doc.TypeId
                                                             select th.IdMacro).Distinct().OrderBy(id => id)
                                             let macroNames = (from idMacro in macroIds
                                                               join dt in documentTypes on idMacro equals dt.Id
                                                               select dt.Name).OrderBy(name => name)
                                             select new YearDocumentTypeGroup
                                             {
                                                 Year = yearGroup.Key,
                                                 SubTypeIds = macroIds.ToList(),
                                                 SubTypeNames = macroNames.ToList()
                                             }).OrderBy(y => y.Year).ToList()
                           }).ToList();

            return grouped;
        }

        public async Task<DocumentDetailDto> GetDocument(int Id)
        {
            var document = await _dbContext.Documents.Include(x => x.Institution).Include(x => x.Project).Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.Id == Id);
            var hierarhyId = await _dbContext.DocumentTypeHierarchies.AsNoTracking().FirstOrDefaultAsync(x => x.IdMicro == document.TypeId);
            var documentMacroType = await _dbContext.DocumentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == hierarhyId.IdMacro);

            return new DocumentDetailDto
            {
                File = document.SavedPath,
                Institution = document.Institution?.Name,
                MacroType = documentMacroType?.Name,
                MicroType = document.DocumentType?.Name,
                Project = document.Project?.Name,
                DataGroup = document.GroupingDate.ToString("yyyy-MM-dd"),
                AdditionalInfo = document.AdditionalInfo,
            };


        }

        public async Task<IEnumerable<DocumentTypeDto>> GetMacroDocumentType()
        {
            return await _dbContext.DocumentTypes.Where(x => x.IsMacro).Select(x => new DocumentTypeDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }

        public async Task<IEnumerable<DocumentTypeDto>> GetMicroTypesByMacroId(int macroId)
        {
            var microTypesId = await _dbContext.DocumentTypeHierarchies
                                               .Where(x => x.IdMacro == macroId)
                                               .Select(x => x.IdMicro)
                                               .ToListAsync();

            
            return  await _dbContext.DocumentTypes
                                                .Where(x => microTypesId.Contains(x.Id))
                                                .Select(t => new DocumentTypeDto
                                                {
                                                    Id = t.Id,
                                                    Name = t.Name,
                                                }).ToListAsync();
        }
    }
}
