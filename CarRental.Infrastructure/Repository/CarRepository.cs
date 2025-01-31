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

    public async Task<Car?> GetById(int id, CancellationToken cancellation)
        => await _context.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellation);

    public async Task<ICollection<Car>> GetAll(CancellationToken cancellation)
        => await _context.Cars.ToListAsync(cancellation);

    public async Task Commit() => await _context.SaveChangesAsync();

    public async Task Remove(int id, CancellationToken cancellation)
    {
        var carToRemove = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellation);

        _context.Cars.Remove(carToRemove!); // Handler has exception for null id

        await _context.SaveChangesAsync(cancellation);
    }
}
