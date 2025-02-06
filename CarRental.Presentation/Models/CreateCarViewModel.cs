namespace CarRental.Presentation.Models;

public class CreateCarViewModel
{
    public int Id { get; set; }
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public decimal PricePerDay { get; set; }
    public bool IsAvailable { get; set; }

    public List<IFormFile> Images { get; set; } = new List<IFormFile>();

    public List<string> ImageUrls { get; set; } = new List<string>();

    public string Description { get; set; } = default!;
    public string? Engine { get; set; }
    public int Year { get; set; }
}
