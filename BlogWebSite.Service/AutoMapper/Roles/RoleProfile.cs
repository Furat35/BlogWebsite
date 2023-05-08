using AutoMapper;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Roles;

namespace BlogWebSite.Service.AutoMapper.Roles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<AppRole, RoleDto>().ReverseMap();
            CreateMap<AppRole, RoleAddDto>().ReverseMap();
            CreateMap<AppRole, RoleUpdateDto>().ReverseMap();
        }
    }
}
