namespace Backend.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Rating { get; set; }
        public List<string> Images { get; set; }
        public int CategoryId {  get; set; }
        public int ProviderId { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Slot> Slots { get; set; }
        public ICollection<BookedSlot> BookedSlots { get; set; }
    }
}
