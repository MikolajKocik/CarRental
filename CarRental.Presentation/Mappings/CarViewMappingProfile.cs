using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Presentation.Models;

namespace CarRental.Presentation.Mappings;

public class CarViewMappingProfile : Profile
{
    public CarViewMappingProfile()
    {
        CreateMap<CarDto, CreateCarViewModel>()
             .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImagePaths));

        CreateMap<CreateCarViewModel, CarDto>()
            .ForMember(dest => dest.ImagePaths, opt => opt.Ignore()); 

        CreateMap<CarDto, EditCarViewModel>()
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImagePaths));

        CreateMap<EditCarViewModel, CarDto>()
            .ForMember(dest => dest.ImagePaths, opt => opt.Ignore());
    }
}
