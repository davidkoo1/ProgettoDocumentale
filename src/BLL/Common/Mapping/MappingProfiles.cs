using AutoMapper;
using BLL.DTO;
using BLL.DTO.InstitutionDTOs;
using BLL.UserDTOs;
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
            CreateMap<User, UserDto>().ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
                                        .ForMember(d => d.InstitutionName, o => o.MapFrom(s => s.Institution.Name))
                                        .ForMember(d => d.InstitutionId, o => o.MapFrom(s => s.Institution.Id));
            CreateMap<UserDto, User>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UpdateUserDto>().ForMember(dest => dest.RolesId, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Id)));
            CreateMap<UpdateUserDto, User>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();


            CreateMap<Institution, InstitutionDto>();
            CreateMap<InstitutionDto, Institution>();

            CreateMap<CreateInstitutionDto, Institution>();
            CreateMap<Institution, UpdateInstitutionDto>();//GetForUpdate
            CreateMap<UpdateInstitutionDto, Institution>();//DtoToUpdate
        }
    }

}
