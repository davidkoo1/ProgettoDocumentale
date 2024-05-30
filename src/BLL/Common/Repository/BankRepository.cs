using AutoMapper;
using BLL.Common.Interfaces;
using BLL.DTO.DocumentDTOs;
using DAL.Context.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
using DAL.Entities;
using BLL.DTO.BankDTOs;

namespace BLL.Common.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BankRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SLAGroupTreeVm>> GetAllSLAThree(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var institutionId = user.IdInstitution;

            var typeHierarchy = await _dbContext.DocumentTypeHierarchies
                .Where(x => x.IdMacro == 2)
                .Select(t => t.IdMicro)
                .ToListAsync();

            var documents = await _dbContext.Documents
                .Include(doc => doc.Institution)
                .Include(doc => doc.DocumentType)
                .Where(x => typeHierarchy.Contains(x.TypeId) && x.InstitutionId == institutionId)
                .ToListAsync();

            var grouped = documents
                .GroupBy(doc => new { doc.GroupingDate.Year, doc.GroupingDate.Month })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Count = group.Count(),
                    Reports = group.Select(d => new ReportVm { Id = d.Id, Text = d.Name }).ToList()
                }).ToList();

            var result = grouped.GroupBy(group => group.Year)
                .Select(yearGroup => new SLAGroupTreeVm
                {
                    Year = yearGroup.Key,
                    Months = yearGroup.Select(monthGroup => new MonthSLAGroup
                    {
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Month),
                        Count = monthGroup.Count,
                        Reports = monthGroup.Reports.Select(report => new ReportVm
                        {
                            Text = report.Text,
                            Id = report.Id
                        }).ToList()
                    }).ToList()
                }).ToList();

            return result;
        }


        public async Task<List<ServiceGroupTreeVm>> GetAllSerciceThree(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var institutionId = user.IdInstitution;

            var typeHierarchy = await _dbContext.DocumentTypeHierarchies
                .Where(x => x.IdMacro == 1)
                .Select(t => t.IdMicro)
                .ToListAsync();

            var documents = await _dbContext.Documents
                .Include(doc => doc.Institution)
                .Include(doc => doc.DocumentType)
                .Where(x => typeHierarchy.Contains(x.TypeId) && x.InstitutionId == institutionId)
                .ToListAsync();

            var grouped = documents
                .GroupBy(doc => new { doc.GroupingDate.Year, doc.GroupingDate.Month, doc.DocumentType.Name })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    DocumentType = group.Key.Name,
                    Count = group.Count(),
                    Reports = group.Select(d => new ReportVm { Id = d.Id, Text = d.Name }).ToList()
                }).ToList();


            var result = grouped.GroupBy(group => group.Year)
                .Select(yearGroup => new ServiceGroupTreeVm
                {
                    Year = yearGroup.Key,
                    Months = yearGroup.GroupBy(group => group.Month)
                        .Select(monthGroup => new MonthServiceGroup
                        {
                            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                            DocumentTypes = monthGroup.Select(m => new DocumentTypeGroup
                            {
                                Name = m.DocumentType,
                                Count = m.Count,
                                Reports = m.Reports.Select(report => new ReportVm
                                {
                                    Text = report.Text,
                                    Id = report.Id
                                }).ToList()
                            }).ToList()
                        }).ToList()
                }).ToList();

            return result;
        }

        public Task<List<ServiceGroupTreeVm>> GetAllProjectThree(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<DownloadFile> GetFileById(int id)
        {
            var document = await _dbContext.Documents.FindAsync(id);
            return new DownloadFile { Name = document.Name, Path = document.SavedPath};
        }
    }
}
