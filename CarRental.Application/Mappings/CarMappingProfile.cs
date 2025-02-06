using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.ImagePaths, opt => opt.MapFrom(src => src.Images.Select(i => i.Path).ToList()))
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<CarDto, Car>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                src.ImagePaths.Select(path => new CarImage { Path = path, FileName = System.IO.Path.GetFileName(path) }).ToList()))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
