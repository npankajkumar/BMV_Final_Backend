namespace Backend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5).AddMinutes(30);
        public int CustomerId { get; set; }
        public int ProviderId { get; set; }
        public int VenueId { get; set; }
        public double Amount { get; set; }
        public DateOnly Date { get;set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public ICollection<BookedSlot> BookedSlots { get; set; }    
    }
}
