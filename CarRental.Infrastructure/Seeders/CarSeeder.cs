using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CarRental.Infrastructure.Seeder;

public class CarSeeder : ICarSeeder
{
    private readonly ApplicationDbContext _context;

    public CarSeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task SeedCarAsync(ILogger logger)
    {

        if (await _context.Database.CanConnectAsync())
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_context.Cars.Any())
                    {
                        _context.Cars.AddRange(
                            new Car
                            {
                                Brand = "Toyota",
                                Model = "Corolla",
                                PricePerDay = 120,
                                IsAvailable = true,
                                ImageUrl = "/Images/toyota_corolla.jpg",
                                Year = 2022,
                                Engine = "1.6 Petrol",
                                Description = "Reliable city sedan with low fuel consumption and a comfortable interior."
                            },
                            new Car
                            {
                                Brand = "Volkswagen",
                                Model = "Golf",
                                PricePerDay = 150,
                                IsAvailable = true,
                                ImageUrl = "/Images/volkswagen_golf.jpg",
                                Year = 2021,
                                Engine = "1.5 TSI",
                                Description = "Popular compact car for city driving and long trips, renowned for its versatility and fuel efficiency."
                            },
                            new Car
                            {
                                Brand = "Tesla",
                                Model = "Model 3",
                                PricePerDay = 400,
                                IsAvailable = true,
                                ImageUrl = "/Images/tesla.jpg",
                                Year = 2023,
                                Engine = "Electric",
                                Description = "An electric innovation with impressive acceleration and modern features."
                            },
                            new Car
                            {
                                Brand = "Toyota",
                                Model = "Rav4",
                                PricePerDay = 200,
                                IsAvailable = true,
                                ImageUrl = "/Images/toyota_rav4.jpg",
                                Year = 2023,
                                Engine = "2.0 Hybrid",
                                Description = "A versatile SUV featuring a hybrid powertrain and a spacious interior."
                            },
                            new Car
                            {
                                Brand = "BMW",
                                Model = "X5",
                                PricePerDay = 300,
                                IsAvailable = true,
                                ImageUrl = "/Images/bmw_x5.jpg",
                                Year = 2021,
                                Engine = "3.0 Diesel",
                                Description = "A luxurious SUV with a sporty character, ensuring both comfort and high performance."
                            },
                            new Car
                            {
                                Brand = "Fiat",
                                Model = "Punto",
                                PricePerDay = 80,
                                IsAvailable = true,
                                ImageUrl = "/Images/fiat_punto.jpg",
                                Year = 2019,
                                Engine = "1.2 Petrol",
                                Description = "A compact, economical car ideal for urban driving and easy parking."
                            },
                            new Car
                            {
                                Brand = "Mercedes",
                                Model = "C-Class",
                                PricePerDay = 350,
                                IsAvailable = true,
                                ImageUrl = "/Images/mercedes_c-class.jpg",
                                Year = 2021,
                                Engine = "2.0 Diesel",
                                Description = "An elegant sedan combining luxury and modern technology for everyday driving."
                            },
                            new Car
                            {
                                Brand = "Ford",
                                Model = "Focus",
                                PricePerDay = 100,
                                IsAvailable = true,
                                ImageUrl = "/Images/ford_focus.jpg",
                                Year = 2020,
                                Engine = "1.0 EcoBoost",
                                Description = "A spacious compact car with a dynamic engine, perfect for both urban and highway driving."
                            },
                            new Car
                            {
                                Brand = "Audi",
                                Model = "A4",
                                PricePerDay = 250,
                                IsAvailable = true,
                                ImageUrl = "/Images/audi_a4.jpg",
                                Year = 2022,
                                Engine = "2.0 TFSI",
                                Description = "A modern sedan featuring a powerful engine and a comfortable interior, perfect for long trips."
                            }
                        );
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError(ex, "There was a problem during seeding a car data");
                    throw;
                }
            }
        }
    }
}
