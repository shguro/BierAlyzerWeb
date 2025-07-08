using AutoMapper;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Models;

namespace BierAlyzer.Api.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
