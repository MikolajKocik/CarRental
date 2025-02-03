using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile()
        {
            CreateMap<Car, CarDto>();

            CreateMap<CarDto, Car>();
        }
    }
}
