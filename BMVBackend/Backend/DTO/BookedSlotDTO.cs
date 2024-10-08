namespace Backend.DTO
{
    public class BookedSlotDTO
    {
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public int BookingId { get; set; }
        public int SlotId { get; set; }
    }
}
