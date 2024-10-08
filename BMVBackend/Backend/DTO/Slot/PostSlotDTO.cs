namespace Backend.DTO.Slot
{
    public class PostSlotDTO
    {
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public int DurationInMinutes { get; set; } 
        public double WeekdayPrice { get; set; }
        public double WeekendPrice { get; set; }
    }
}
