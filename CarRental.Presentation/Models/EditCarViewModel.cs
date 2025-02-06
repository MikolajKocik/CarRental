namespace CarRental.Presentation.Models;

public class EditCarViewModel
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public decimal PricePerDay { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; } = default!;
    public string? Engine { get; set; }
    public int Year { get; set; }

    public List<IFormFile> NewImages { get; set; } = new List<IFormFile>();

    public List<string> ImageUrls { get; set; } = new List<string>();
}
