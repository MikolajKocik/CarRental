using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarRental.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]  
        [DataType(DataType.Date)] 
        public DateTime StartDate { get; set; }
        [Required]  
        [DataType(DataType.Date)] 
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalCost { get; set; }

        public string UserId { get; set; } = default!; // Identity user

        public int CarId { get; set; }
        public Car Car { get; set; } = default!;
    }
}
