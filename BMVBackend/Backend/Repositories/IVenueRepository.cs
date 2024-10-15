using Backend.Models;

namespace Backend.Repositories
{
    public interface IVenueRepository
    {
        void AddCategoryIfNotExist(Category category);
        void AddSlot(Slot slot);
        void AddVenue(Venue venue);
        IQueryable<Venue> GetAllVenues();
        Category GetCategoryByName(string categoryName);
        IQueryable<Venue> GetTopBookedVenues();
        IQueryable<Venue> GetTopRatedVenues();
        Venue GetVenueById(int id);
        void RemoveVenue(Venue venue);
        void SaveChanges();
    }
}