using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.CarName.Split(' ', StringSplitOptions.None)[0]))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.CarName.Split(' ', StringSplitOptions.None)[1]))
                .ForMember(dest => dest.ReservationsCount, opt => opt.MapFrom(src => src.ReservationsCount))
                .ForMember(dest => dest.TotalIncome, opt => opt.MapFrom(src => src.TotalIncome));

            CreateMap<ReportDto, Report>()
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => $"{src.Brand} {src.Model}"))
                .ForMember(dest => dest.ReservationsCount, opt => opt.MapFrom(src => src.ReservationsCount))
                .ForMember(dest => dest.TotalIncome, opt => opt.MapFrom(src => src.TotalIncome));

        }
    }

}
