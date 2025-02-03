using CarRental.Presentation.Mappings;

namespace CarRental.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(CarViewMappingProfile));

            services.AddAutoMapper(typeof(ReservationViewMappingProfile));

            services.AddAutoMapper(typeof(AdminViewMappingProfile)); 

        }
    }
}
