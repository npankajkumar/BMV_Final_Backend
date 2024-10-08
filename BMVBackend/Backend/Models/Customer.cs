using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Mobile), IsUnique = true)]
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [MaxLength(10), MinLength(10)]
        public string Mobile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Booking> Bookings { get; set; }
    }
}
