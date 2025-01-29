using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces
{
    public interface ICarRepository
    {
        // Index ?
        Task Create (Car car);

        Task Details(int id);
    }
}
