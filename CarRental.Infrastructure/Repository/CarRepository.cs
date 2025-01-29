using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repository;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Create(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }

    public Task Details(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Car>> GetAll()
        => await _context.Cars.ToListAsync();
}
