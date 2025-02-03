using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Presentation.Models;

namespace CarRental.Presentation.Mappings;

public class CarViewMappingProfile : Profile
{
    public CarViewMappingProfile()
    {
        CreateMap<CarDto, CreateCarViewModel>().ReverseMap();

        CreateMap<CarDto, EditCarViewModel>().ReverseMap();
    }
}
