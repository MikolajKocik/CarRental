using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>();

            CreateMap<CarDto, CarDto>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // UserName must be assign to database
                .ForMember(dest => dest.Car, opt => opt.MapFrom(src => new CarDto
                {
                    Id = src.Car.Id,
                    Brand = src.Car.Brand,
                    Model = src.Car.Model,
                    PricePerDay = src.Car.PricePerDay,
                    IsAvailable = src.Car.IsAvailable,
                    ImageUrl = src.Car.ImageUrl,
                    Description = src.Car.Description,
                    Engine = src.Car.Engine,
                    Year = src.Car.Year,
                }));

            CreateMap<ReservationDto, Reservation>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserName))
              .ForMember(dest => dest.Car, opt => opt.MapFrom(src => new CarDto
              {
                  Id = src.Car.Id,
                  Brand = src.Car.Brand,
                  Model = src.Car.Model,
                  PricePerDay = src.Car.PricePerDay,
                  IsAvailable = src.Car.IsAvailable,
                  ImageUrl = src.Car.ImageUrl,
                  Description = src.Car.Description,
                  Engine = src.Car.Engine,
                  Year = src.Car.Year,
              }));


        }

    }
}
