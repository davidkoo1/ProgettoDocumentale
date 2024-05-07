using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UpdateUserDto>().ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.UserRole.RoleId));
            CreateMap<UpdateUserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }

}
