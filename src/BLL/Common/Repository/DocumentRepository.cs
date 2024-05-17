using AutoMapper;
using BLL.Common.Extensions;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using BLL.TableParameters;
using DAL.Context.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<DocumentItem>> GetAllDocuments(DataTablesParameters parameters)
        {
            var documents = await _dbContext.Documents.Search(parameters).OrderBy(parameters).Page(parameters)
                .Include(x => x.Institution).Search(parameters).OrderBy(parameters).Page(parameters)
                .Include(x => x.DocumentType).Search(parameters).OrderBy(parameters).Page(parameters)
                .ToListAsync();

            var documentItems = documents.Select(d => new DocumentItem
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.DocumentType.Name,
                Date = d.GroupingDate,
                Institution = d.Institution.Name
            }).ToList();

            return documentItems;
        }

        public async Task<IEnumerable<DocumentViewModel>> GetAllThree()
        {
            var documents = await _dbContext.Documents
                .Include(x => x.Institution)
                .Include(x => x.DocumentType)
                .ToListAsync();

            var groupedDocuments = documents.GroupBy(d => new { d.Institution.Name, d.GroupingDate.Year })
                                        .Select(g => new DocumentViewModel
                                        {
                                            Institution = g.Key.Name,
                                            Year = g.Key.Year,
                                            Documents = g.Select(d => new DocumentItem
                                            {
                                                Name = d.Name,
                                                Type = d.DocumentType.Name,
                                                Date = d.GroupingDate,
                                                Institution = d.Institution.Name,
                                                //Project = d.AdditionalInfo
                                            }).ToList()
                                        }).ToList();

            return groupedDocuments;
        }
    }
}
