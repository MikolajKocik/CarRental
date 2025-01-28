using System.ComponentModel.DataAnnotations;

namespace Wypożyczalnia_samochodów_online.Models
{
    public class Car
    {
        public int Id { get; set; } // Główny klucz identyfikujący pojazd
        public string Brand { get; set; } // Marka samochodu, np. "Toyota"
        public string Model { get; set; } // Model samochodu, np. "Corolla"
        public decimal PricePerDay { get; set; } // Cena wynajmu za dzień
        public bool IsAvailable { get; set; } // Status dostępności pojazdu

        [Required(ErrorMessage = "Ścieżka obrazu jest wymagana.")]
        [Url(ErrorMessage = "Niepoprawny format URL.")]
        public string ImageUrl { get; set; } // URL zdjęcia
        public string Description { get; set; } // Kilka słów o tym konkretnym modelu
        public string Engine { get; set; }  // np. 1.8L, 2.0 TDi


        [Range(2010, 2025, ErrorMessage = "Rok produkcji musi być pomiędzy 2010 a 2025.")]
        public int Year { get; set; }   // Rok produkcji


        // Nawigacja z rezerwacjami 
        public ICollection<Reservation> Reservations { get; set; }
    }
}
