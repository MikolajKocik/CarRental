using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CarRental.Infrastructure.Seeders;

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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!_context.Cars.Any())
                    {
                        var cars = new List<Car>
                        {
                            new Car
                            {
                                Brand = "Toyota",
                                Model = "Corolla",
                                PricePerDay = 120,
                                IsAvailable = true,
                                Year = 2022,
                                Engine = "1.6 Petrol",
                                Description = "Reliable city sedan with low fuel consumption and a comfortable interior.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/toyota_corolla.jpg", FileName = "toyota_corolla.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Volkswagen",
                                Model = "Golf",
                                PricePerDay = 150,
                                IsAvailable = true,
                                Year = 2021,
                                Engine = "1.5 TSI",
                                Description = "Popular compact car for city driving and long trips, renowned for its versatility and fuel efficiency.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/volkswagen_golf.jpg", FileName = "volkswagen_golf.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Tesla",
                                Model = "Model 3",
                                PricePerDay = 400,
                                IsAvailable = true,
                                Year = 2023,
                                Engine = "Electric",
                                Description = "An electric innovation with impressive acceleration and modern features.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/tesla.jpg", FileName = "tesla.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Toyota",
                                Model = "Rav4",
                                PricePerDay = 200,
                                IsAvailable = true,
                                Year = 2023,
                                Engine = "2.0 Hybrid",
                                Description = "A versatile SUV featuring a hybrid powertrain and a spacious interior.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/toyota_rav4.jpg", FileName = "toyota_rav4.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "BMW",
                                Model = "X5",
                                PricePerDay = 300,
                                IsAvailable = true,
                                Year = 2021,
                                Engine = "3.0 Diesel",
                                Description = "A luxurious SUV with a sporty character, ensuring both comfort and high performance.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/bmw_x5.jpg", FileName = "bmw_x5.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Fiat",
                                Model = "Punto",
                                PricePerDay = 80,
                                IsAvailable = true,
                                Year = 2019,
                                Engine = "1.2 Petrol",
                                Description = "A compact, economical car ideal for urban driving and easy parking.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/fiat_punto.jpg", FileName = "fiat_punto.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Mercedes",
                                Model = "C-Class",
                                PricePerDay = 350,
                                IsAvailable = true,
                                Year = 2021,
                                Engine = "2.0 Diesel",
                                Description = "An elegant sedan combining luxury and modern technology for everyday driving.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/mercedes_c-class.jpg", FileName = "mercedes_c-class.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Ford",
                                Model = "Focus",
                                PricePerDay = 100,
                                IsAvailable = true,
                                Year = 2020,
                                Engine = "1.0 EcoBoost",
                                Description = "A spacious compact car with a dynamic engine, perfect for both urban and highway driving.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/ford_focus.jpg", FileName = "ford_focus.jpg" }
                                }
                            },
                            new Car
                            {
                                Brand = "Audi",
                                Model = "A4",
                                PricePerDay = 250,
                                IsAvailable = true,
                                Year = 2022,
                                Engine = "2.0 TFSI",
                                Description = "A modern sedan featuring a powerful engine and a comfortable interior, perfect for long trips.",
                                Images = new List<CarImage>
                                {
                                    new CarImage { Path = "/images/audi_a4.jpg", FileName = "audi_a4.jpg" }
                                }
                            }
                        };

                        foreach (var car in cars)
                        {
                            var existingCar = _context.Cars
                                .FirstOrDefault(c => c.Brand == car.Brand && c.Model == car.Model);

                            if (existingCar != null)
                            {
                                existingCar.PricePerDay = car.PricePerDay;
                                existingCar.IsAvailable = car.IsAvailable;
                                existingCar.Year = car.Year;
                                existingCar.Engine = car.Engine;
                                existingCar.Description = car.Description;

                                if (car.Images != null && car.Images.Any())
                                {
                                    existingCar.Images = car.Images;
                                }
                            }
                            else
                            {
                                // Dodaj nowy rekord
                                await _context.Cars.AddAsync(car);
                            }
                        }

                        await _context.Cars.AddRangeAsync(cars);
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
