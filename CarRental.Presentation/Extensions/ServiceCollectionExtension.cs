using CarRental.Mappings;

namespace CarRental.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(PresentationProfile));
        }
    }
}
