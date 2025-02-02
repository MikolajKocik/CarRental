using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Presentation.Models;

namespace CarRental.Mappings
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<AdminReportsDto, AdminReportsViewModel>();
        }
    }

}
