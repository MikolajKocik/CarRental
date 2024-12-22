namespace Wypożyczalnia_samochodów_online.Models
{
    public class Car
    {
        public int Id { get; set; } // Główny klucz identyfikujący pojazd
        public string Brand { get; set; } // Marka samochodu, np. "Toyota"
        public string Model { get; set; } // Model samochodu, np. "Corolla"
        public decimal PricePerDay { get; set; } // Cena wynajmu za dzień
        public bool IsAvailable { get; set; } // Status dostępności pojazdu
        public string ImageUrl { get; set; } // URL zdjęcia

        // Nawigacja z rezerwacjami 
        public ICollection<Reservation> Reservations { get; set; }
    }
}
