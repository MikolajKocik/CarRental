using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces.Repositories
{
    public interface IHomeRepository
    {
        Task<ICollection<Car>> GetPopularCars(CancellationToken cancellation);
    }
}
