using BLL.DTO.BankDTOs;
using BLL.DTO.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Common.Repository.BankRepository;

namespace BLL.Common.Interfaces
{
    public interface IBankRepository
    {
        Task<List<YearGroup>> GetAllThree();
        Task<DownloadFile> GetFileById(int id);
    }
}
