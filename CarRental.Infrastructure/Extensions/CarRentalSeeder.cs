using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Seeder;
using CarRental.Infrastructure.Seeders;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRental.Infrastructure.Extensions
{
    public class CarRentalSeeder : ICarRentalSeeder
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IdentityUser> _logger;
        private readonly ILogger _carLogger;

        public CarRentalSeeder(RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> userManager,
            ApplicationDbContext context, ILogger<IdentityUser> logger, ILogger carLogger)
        {
            _rolemanager = rolemanager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _carLogger = carLogger;
        }
        public async Task MigrateDatabase()
        {
            await _context.Database.MigrateAsync();
        }
        public async Task SeedIdentityAsync()
        {
            await IdentitySeeder.SeederIdentityAsync(_context, _rolemanager, _userManager, _logger);
        }

        public async Task SeedCarAsync()
        {
            await CarSeeder.SeedCarAsync(_context, _carLogger);
        }
    
    }
}
