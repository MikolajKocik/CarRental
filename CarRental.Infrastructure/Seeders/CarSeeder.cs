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
                                Engine = "1.6 Benzyna",
                                Description = "Niezawodny, miejski sedan o niskim spalaniu i wygodnym wnętrzu."
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
                                Description = "Popularny kompakt do miasta i na trasę, ceniony za uniwersalność i oszczędność."
                            },
                            new Car
                            {
                                Brand = "Tesla",
                                Model = "Model 3",
                                PricePerDay = 400,
                                IsAvailable = true,
                                ImageUrl = "/Images/tesla.jpg",
                                Year = 2023,
                                Engine = "Elektryczny",
                                Description = "Elektryczna innowacja z imponującym przyspieszeniem i nowoczesnymi rozwiązaniami."
                            },
                            new Car
                            {
                                Brand = "Toyota",
                                Model = "Rav4",
                                PricePerDay = 200,
                                IsAvailable = true,
                                ImageUrl = "/Images/toyota_rav4.jpg",
                                Year = 2023,
                                Engine = "2.0 Hybryda",
                                Description = "Wszechstronny SUV z napędem hybrydowym i przestronnym wnętrzem."
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
                                Description = "Luksusowy SUV o sportowym charakterze, gwarantuje komfort i wysokie osiągi."
                            },
                            new Car
                            {
                                Brand = "Fiat",
                                Model = "Punto",
                                PricePerDay = 80,
                                IsAvailable = true,
                                ImageUrl = "/Images/fiat_punto.jpg",
                                Year = 2019,
                                Engine = "1.2 Benzyna",
                                Description = "Niewielkie, ekonomiczne auto idealne do jazdy miejskiej i łatwego parkowania."
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
                                Description = "Elegancka limuzyna łącząca luksus i nowoczesne technologie w codziennej jeździe."
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
                                Description = "Przestronny kompakt z dynamicznym silnikiem, świetnie sprawdza się w mieście i na trasach."
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
                                Description = "Nowoczesny sedan z mocnym silnikiem i komfortowym wnętrzem, idealny na długie podróże."
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
