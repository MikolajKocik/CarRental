using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Entities;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public decimal PricePerDay { get; set; } 
    public bool IsAvailable { get; set; } 

    [Required(ErrorMessage = "Ścieżka obrazu jest wymagana.")]
    [Url(ErrorMessage = "Niepoprawny format URL.")]
    public string ImageUrl { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Engine { get; set; } = default!;


    [Range(2010, 2025, ErrorMessage = "Rok produkcji musi być pomiędzy 2010 a 2025.")]
    public int Year { get; set; } 


    // Navigation to reservation
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
