namespace Backend.Models
{
    public class BookedSlot
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int VenueId { get; set; }
        public int BookingId { get; set; }
        public int SlotId { get; set; }
    }
}
