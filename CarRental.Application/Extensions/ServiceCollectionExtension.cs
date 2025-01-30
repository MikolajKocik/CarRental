using CarRental.Application.Dto.CreateCar;
using CarRental.Application.Dto.CreateReservation;
using CarRental.Application.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(CreateCarCommand));
            services.AddMediatR(typeof(CreateReservationCommand));

            services.AddAutoMapper(typeof(CarMappingProfile));
            services.AddAutoMapper(typeof(ReservationMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateCarCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateReservationCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }
    }
}
