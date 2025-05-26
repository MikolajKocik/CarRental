using CarRental.Application.CQRS.Commands.Car.CreateCar;
using CarRental.Application.CQRS.Commands.Car.DeleteCar;
using CarRental.Application.CQRS.Commands.Car.EditCar;
using CarRental.Application.CQRS.Commands.Reservation.ConfirmReservation;
using CarRental.Application.CQRS.Commands.Reservation.CreateReservation;
using CarRental.Application.CQRS.Queries.AdminQueries;
using CarRental.Application.CQRS.Queries.CarQueries.GetAllCars;
using CarRental.Application.CQRS.Queries.CarQueries.GetCarDetails;
using CarRental.Application.CQRS.Queries.CarQueries.GetPopularCars;
using CarRental.Application.CQRS.Queries.ReservationQueries.GetMyReservations;
using CarRental.Application.CQRS.Queries.ReservationQueries.GetReservationDetails;
using CarRental.Application.Mappings;
using CarRental.Application.Services;
using CarRental.Domain.Interfaces.Services;
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
            services.AddMediatR(typeof(ConfirmReservationCommand));

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

            services.AddValidatorsFromAssemblyContaining<ConfirmReservationCommand>()
              .AddFluentValidationAutoValidation()
              .AddFluentValidationClientsideAdapters();

            services.AddMediatR(typeof(GetReportQuery));
            services.AddMediatR(typeof(GetAllCarsQuery));
            services.AddMediatR(typeof(CarDetailsQuery));
            services.AddMediatR(typeof(GetPopularCarsQuery));
            services.AddMediatR(typeof(GetMyReservationQuery));
            services.AddMediatR(typeof(GetReservationDetailsQuery));

            services.AddTransient<ISmtpClient, SmtpClientWrapper>(); // lub inna implementacja

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileService, FileService>();
        }
    }
}
