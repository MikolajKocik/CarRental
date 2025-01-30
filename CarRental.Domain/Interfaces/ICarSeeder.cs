using Microsoft.Extensions.Logging;

namespace CarRental.Domain.Interfaces;

public interface ICarSeeder
{
    Task SeedCarAsync(ILogger logger);
}
