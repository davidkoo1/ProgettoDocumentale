using BLL.DTO.BankDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IBankRepository
    {
        Task<List<ServiceGroupTreeVm>> GetAllSerciceThree(int userId);
        Task<List<SLAGroupTreeVm>> GetAllSLAThree(int userId);
        Task<List<ProjectGroupTreeVm>> GetAllProjectThree(int userId);
        Task<DownloadFile> GetFileById(int id);
    }
}
