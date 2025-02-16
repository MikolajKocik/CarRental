using CarRental.Application.Dto.CreateCar;
using CarRental.Application.Dto.CreateReservation;
using CarRental.Application.Dto.DeleteCar;
using CarRental.Application.Dto.EditCar;
using CarRental.Application.Dto.Queries.AdminQueries;
using CarRental.Application.Dto.Queries.CarQueries.GetAllCars;
using CarRental.Application.Dto.Queries.CarQueries.GetCarDetails;
using CarRental.Application.Dto.Queries.CarQueries.GetPopularCars;
using CarRental.Application.Dto.Queries.ReservationQueries.GetMyReservations;
using CarRental.Application.Dto.Queries.ReservationQueries.GetReservationDetails;
using CarRental.Application.Mappings;
using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
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
            services.AddMediatR(typeof(DeleteCarCommand));
            services.AddMediatR(typeof(EditCarCommand));

            services.AddAutoMapper(typeof(CarMappingProfile));
            services.AddAutoMapper(typeof(ReservationMappingProfile));
            services.AddAutoMapper(typeof(ReportMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateCarCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateReservationCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<DeleteCarCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<EditCarCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddMediatR(typeof(GetReportQuery));
            services.AddMediatR(typeof(GetAllCarsQuery));
            services.AddMediatR(typeof(CarDetailsQuery));
            services.AddMediatR(typeof(GetPopularCarsQuery));
            services.AddMediatR(typeof(GetMyReservationQuery));
            services.AddMediatR(typeof(GetReservationDetailsQuery));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileService, FileService>();
        }
    }
}
