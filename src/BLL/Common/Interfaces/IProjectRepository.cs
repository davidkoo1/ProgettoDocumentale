using BLL.DTO.DocumentDTOs;
using BLL.DTO.ProjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<BuilderProjectThree>> GetAllThree();
    }
}
