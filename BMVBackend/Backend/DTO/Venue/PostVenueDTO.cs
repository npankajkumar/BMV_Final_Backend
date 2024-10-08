using Backend.DTO.Slot;

namespace Backend.DTO.Venue
{
    public class PostVenueDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public List<IFormFile> Images { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Category { get; set; }
        public PostSlotDTO slotDetails {  get; set; }
    }
}
