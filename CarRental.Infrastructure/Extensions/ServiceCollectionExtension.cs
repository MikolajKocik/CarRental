using CarRental.Application.Services;
using CarRental.Domain.Interfaces.Repositories;
using CarRental.Domain.Interfaces.Seeders;
using CarRental.Domain.Interfaces.Services;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Repository;
using CarRental.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CarRental.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICarRepository, CarRepository>();

        services.AddScoped<IReservationRepository, ReservationRepository>();

        services.AddScoped<IHomeRepository, HomeRepository>();

        services.AddScoped<IAdminRepository, AdminRepository>();

        services.AddTransient<IIdentitySeeder, IdentitySeeder>();

        services.AddTransient<ICarSeeder, CarSeeder>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
