using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Presentation.Models;


namespace CarRental.Presentation.Mappings;

public class ReservationViewMappingProfile : Profile
{
    public ReservationViewMappingProfile()
    {
        CreateMap<ReservationDto, MyReservationsViewModel>()
            .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car));
    }
}
