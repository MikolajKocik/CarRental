using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task Create (Car car);

        Task<ICollection<Car>> GetAll(CancellationToken cancellation);

        Task<Car> GetById(int id, CancellationToken cancellation);

        Task Commit();
    }
}
