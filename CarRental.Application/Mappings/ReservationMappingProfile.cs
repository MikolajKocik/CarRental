using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Mappings
{
    public class ReservationMappingProfile : Profile
    {
        public ReservationMappingProfile()
        {
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // UserName must be assign to database
                .ForMember(dest => dest.Car, opt => opt.MapFrom(src => new CarDto
                {
                    Id = src.Car.Id,
                    Brand = src.Car.Brand,
                    Model = src.Car.Model,
                    PricePerDay = src.Car.PricePerDay,
                    IsAvailable = src.Car.IsAvailable,
                    ImagePaths = src.Car.Images.Select(i => i.Path).ToList(),
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
                  ImagePaths = src.Car.ImagePaths,
                  Description = src.Car.Description,
                  Engine = src.Car.Engine,
                  Year = src.Car.Year,
              }));
        }
    }
}
