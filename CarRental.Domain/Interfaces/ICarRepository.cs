using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task Create (Car car);

        Task<ICollection<Car>> GetAll(CancellationToken cancellation);

        Task<Car> GetDetails(int id, CancellationToken cancellation);
    }
}
