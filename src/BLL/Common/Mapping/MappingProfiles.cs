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
            CreateMap<User, UserDto>().ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
            CreateMap<UserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UpdateUserDto>().ForMember(dest => dest.RolesId, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Id)));
            CreateMap<UpdateUserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }

}
