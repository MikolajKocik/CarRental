using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces
{
    public interface IHomeRepository
    {
        Task<ICollection<Car>> GetPopularCars(CancellationToken cancellation);
    }
}
