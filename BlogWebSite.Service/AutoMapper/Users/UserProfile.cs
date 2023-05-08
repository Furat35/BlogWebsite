using AutoMapper;
using BlogWebSite.Entity.Entities.Concrete;
using BlogWebSite.Entity.Models.DTOs.Users;

namespace BlogWebSite.Service.AutoMapper.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserAddDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserProfileDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<UserRegisterDto, UserAddDto>();
        }
    }
}
