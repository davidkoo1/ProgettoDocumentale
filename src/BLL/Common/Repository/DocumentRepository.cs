using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using BLL.TableParameters;
using DAL.Context.Persistance;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
                .Include(x => x.Project);

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


            var documents = await query.Search(parameters).OrderBy(parameters).Page(parameters).ToListAsync();

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
                    InstitutionId = x.Institution?.Name, 
                    TypeId = typeName,
                    ProjectId = x.Project?.Name ?? " " 
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

            return hierarchyName != null && hierarchyName.Name != documentType.Name ? $"{hierarchyName.Name}/{documentType.Name}" : documentType.Name;
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
            var document = await _dbContext.Documents.Include(x => x.Institution).Include(x => x.Project).Include(x => x.DocumentType).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
            var hierarhyId = await _dbContext.DocumentTypeHierarchies.AsNoTracking().FirstOrDefaultAsync(x => x.IdMicro == document.TypeId);
            var documentMacroType = await _dbContext.DocumentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == hierarhyId.IdMacro);

            return new DocumentDetailDto
            {
                Name = document.Name,
                Institution = document.Institution?.Name,
                MacroType = documentMacroType?.Name,
                MicroType = document.DocumentType?.Name,
                Project = document.Project?.Name,
                DataGroup = document.GroupingDate.ToString("yyyy-MM-dd"),
                AdditionalInfo = document.AdditionalInfo,
                User = document.User.UserName,
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

        public async Task<IEnumerable<ProjectDto>> GetProjectsByInstitutionId(int? InstitutionId)
        {
            return await _dbContext.Projects.Where(x => x.InstitutionId == InstitutionId && x.IsActive).Select(t => new ProjectDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync();
        }

        public async Task<bool> CreateDocument(CreateDocumentDto model)
        {
            var fileName = Path.GetFileName(model.File.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            var baseFilePath = HttpContext.Current.Server.MapPath("~/Access/uploads");

            var bankName = _dbContext.Institutions
                .Where(x => x.Id == model.InstitutionId)
                .Select(x => x.Name)
                .FirstOrDefault();

            var year = model.GroupingDate.Year.ToString();
            var month = model.GroupingDate.ToString("MMMM");

            var pathList = new List<string> { baseFilePath, bankName, year, month };
            var filePath = baseFilePath;

            foreach (var path in pathList)
            {
                filePath = Path.Combine(filePath, path);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            var documentName = fileName;
            var finalPath = Path.Combine(filePath, documentName);
            var count = 1;

            while (File.Exists(finalPath))
            {
                documentName = $"{fileNameWithoutExtension} ({count}){fileExtension}";
                finalPath = Path.Combine(filePath, documentName);
                count++;
            }

            using (var destinationStream = new FileStream(finalPath, FileMode.Create))
            {
                await model.File.InputStream.CopyToAsync(destinationStream);
            }

            var entity = new Document
            {
                InstitutionId = model.InstitutionId,
                UserId = model.UserId,
                ProjectId = model.ProjectId,
                TypeId = model.MicroId ?? model.MacroId,
                AdditionalInfo = model.AdditionalInfo,
                UploadDate = DateTime.Now,
                GroupingDate = model.GroupingDate,
                Name = documentName,
                SavedPath = finalPath
            };

            _dbContext.Documents.Add(entity);

            return await Save();
        }


        public async Task<bool> Save() => await _dbContext.SaveChangesAsync() > 0;

        public async Task<bool> Update(UpdateDocumentDto updateDocumentDto)
        {
            var document = await _dbContext.Documents
                    .FirstAsync(x => x.Id == updateDocumentDto.Id);

            document.InstitutionId = updateDocumentDto.InstitutionId;
            document.TypeId = updateDocumentDto.MicroId ?? updateDocumentDto.MacroId;
            document.GroupingDate = updateDocumentDto.GroupingDate;
            document.AdditionalInfo = updateDocumentDto.AdditionalInfo;
            document.ProjectId = updateDocumentDto.ProjectId;

            _dbContext.Documents.AddOrUpdate(document);

            return await Save();
           // await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<UpdateDocumentDto> GetUpdateDocument(int id)
        {
            // Получаем документ с включенными связанными данными
            var document = await _dbContext.Documents
                .Include(x => x.Project)
                .Include(x => x.Institution)
                .Include(x => x.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (document == null)
            {
                return null;
            }

            // Получаем все типы иерархий документов в память
            var documentMacroType = await _dbContext.DocumentTypeHierarchies.ToListAsync();

            // Находим соответствующий MacroId
            var macroId = documentMacroType
                .Where(x => x.IdMicro == document.TypeId)
                .Select(n => n.IdMacro)
                .FirstOrDefault();

            return new UpdateDocumentDto
            {
                Id = document.Id,
                Name = document.Name,
                AdditionalInfo = document.AdditionalInfo,
                GroupingDate = document.GroupingDate,
                InstitutionId = document.InstitutionId,
                MicroId = document.TypeId,
                MacroId = macroId,
                ProjectId = document.ProjectId,
            };
        }


    }
}
