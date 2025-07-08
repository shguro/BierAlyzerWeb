using AutoMapper;
using BierAlyzer.Api.DTOs;
using BierAlyzer.Api.Models;

namespace BierAlyzer.Api.Mappings
{
    public class DrinkMappingProfile : Profile
    {
        public DrinkMappingProfile()
        {
            CreateMap<Drink, DrinkDto>().ReverseMap();
        }
    }
}
