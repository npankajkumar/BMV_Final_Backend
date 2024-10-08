using Backend.DTO.Venue;
using Backend.Models;

namespace Backend.Services
{
    public interface IVenuesService
    {
        Venue AddVenue(int id, PostVenueDTO venueWithSlotDetails);
        bool DeleteVenue(int id);
        List<Venue> GetAllVenues();
        List<Venue> GetTopBookedVenues();
        List<Venue> GetTopRatedVenues();
        Venue GetVenueById(int id);
        Venue UpdateVenue(int id, PutVenueDTO v);
    }
}