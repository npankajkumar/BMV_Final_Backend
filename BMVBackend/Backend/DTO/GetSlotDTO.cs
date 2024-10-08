namespace Backend.DTO
{
    public class GetSlotDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public double WeekdayPrice { get; set; }
        public double WeekendPrice { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }

    }
}
