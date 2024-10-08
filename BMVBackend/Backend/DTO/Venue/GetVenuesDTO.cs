
namespace Backend.DTO.Venue
{
    public class GetVenuesDTO
    {
        public List<Backend.Models.Venue>? TopRatedVenues { get; set; }
        public List<Backend.Models.Venue>? TopBookedVenues { get; set; }
        public List<Backend.Models.Venue>? Venues { get; set; }
    }
}
