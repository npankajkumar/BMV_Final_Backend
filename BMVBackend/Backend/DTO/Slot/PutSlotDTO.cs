namespace Backend.DTO.Slot
{
    public class PutSlotDTO
    {
        public bool? IsBlocked { get; set; }
        public double? WeekdayPrice { get; set; }
        public double? WeekendPrice { get; set; }
    }
}
