using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Dto
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = default!;
        public string Model { get; set; } = default!;
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();
        public List<IFormFile>? Images { get; set; }
        public string Description { get; set; } = default!;
        public string? Engine { get; set; } = default!;
        public int Year { get; set; }

    }
}
