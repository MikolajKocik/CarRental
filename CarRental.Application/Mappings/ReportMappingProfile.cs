using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportDto>().ReverseMap();
        }
    }
}
