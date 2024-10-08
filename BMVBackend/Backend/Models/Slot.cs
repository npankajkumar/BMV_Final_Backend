namespace Backend.Models
{
    public class Slot
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public double WeekdayPrice { get; set; }
        public double WeekendPrice { get; set; }
        public bool IsBlocked { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public ICollection<BookedSlot> BookedSlots { get; set; }
    }
}
