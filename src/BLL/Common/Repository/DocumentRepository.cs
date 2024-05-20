using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.TableParameters;
using DAL.Context.Persistance;
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

        public async Task<IEnumerable<DocumentViewModel>> GetAllThree()
        {
            var documents = await _dbContext.Documents
                .Include("Institution")
                .Include("DocumentType")
                .Include("DocumentType.Macro") // Включаем Macro
                .ToListAsync();


            var result = documents
                .GroupBy(doc => doc.Institution.Name)
                .Select(instGroup => new DocumentViewModel
                {
                    Institution = instGroup.Key,
                    YearGroups = instGroup
                        .GroupBy(doc => doc.GroupingDate.Year)
                        .Select(yearGroup => new YearGroup
                        {
                            Year = yearGroup.Key,
                            Types = yearGroup
                                .Select(doc => doc.DocumentType.Name)
                                .Distinct()
                                .ToList()
                        }).ToList()
                }).ToList();
            //    var documents = await _dbContext.Documents
            //.Include(x => x.Institution)
            //.Include(x => x.DocumentType)
            //.ToListAsync();

            //    var result = documents
            //        .GroupBy(doc => doc.Institution.Name)
            //        .Select(instGroup => new DocumentViewModel
            //        {
            //            Institution = instGroup.Key,
            //            YearGroups = instGroup
            //                .GroupBy(doc => doc.GroupingDate.Year)
            //                .Select(yearGroup => new YearGroup
            //                {
            //                    Year = yearGroup.Key,
            //                    Documents = yearGroup.Select(doc => new DocumentItem
            //                    {
            //                        Id = doc.Id,
            //                        Name = doc.Name,
            //                        Type = doc.DocumentType.Name,
            //                        Date = doc.GroupingDate
            //                    }).ToList()
            //                }).ToList()
            //        }).ToList();


            return result;
        }

    }
}
