using Microsoft.Extensions.Logging;

namespace CarRental.Domain.Interfaces.Seeders;

public interface ICarSeeder
{
    Task SeedCarAsync(ILogger logger);
}
