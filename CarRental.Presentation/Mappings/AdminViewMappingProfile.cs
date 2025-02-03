using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Presentation.Models;

namespace CarRental.Presentation.Mappings;

public class AdminViewMappingProfile : Profile
{
    public AdminViewMappingProfile()
    {
        CreateMap<AdminReportsDto, AdminReportsViewModel>();
    }
}
