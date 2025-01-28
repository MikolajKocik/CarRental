using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces
{
    public interface ICarRepository
    {
        Task Create (Car car);  
    }
}
