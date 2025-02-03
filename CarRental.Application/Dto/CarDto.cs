using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dto
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = default!;
        public string Model { get; set; } = default!;
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Engine { get; set; } = default!;
        public int Year { get; set; }

    }
}
