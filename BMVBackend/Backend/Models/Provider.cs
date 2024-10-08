using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Mobile), IsUnique = true)]
    public class Provider
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Email { get; set; }
        [MaxLength(10), MinLength(10)]
        public string  Mobile { get; set; }
        
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Venue> Venues {get;set;}
    }
}
