using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;


namespace CarRental.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required] // pole wymagane do utworzenia rezerwacji
        public string UserId { get; set; }

        [ForeignKey("UserId")] // klucz obcy dla tabeli car
        public IdentityUser User { get; set; } 

        [Required] // pole wymagane -,,-
        public int CarId { get; set; }

        [ForeignKey("CarId")] // klucz obcy dla tabeli car
        public Car Car { get; set; }

        [Required]  // pole wymagane -,,- 
        [DataType(DataType.Date)] 
        public DateTime StartDate { get; set; }

        [Required]  // pole wymagane -,,-
        [DataType(DataType.Date)] 
        public DateTime EndDate { get; set; }

        public bool IsConfirmed { get; set; }

        public decimal TotalCost { get; set; }
    }
}
