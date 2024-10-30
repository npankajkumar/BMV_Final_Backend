using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly BmvContext _context;

        public VenueRepository(BmvContext context)
        {
            _context = context;
        }

        public IQueryable<Venue> GetAllVenues()
        {
            return _context.Venues.AsQueryable();
        }

        public IQueryable<Venue> GetTopRatedVenues()
        {
            var topRatedVenues = _context.Venues
        .Select(v => new
        {
            Venue = v,
            AverageRating = v.RatingCount == 0 ? 0 : v.RatingSum / v.RatingCount 
        })
        .Where(v => v.AverageRating >= 4) 
        .OrderByDescending(v => v.AverageRating) 
        .Take(5) 
        .Select(v => v.Venue)
        .ToList();

            return topRatedVenues.AsQueryable();
        
        }

        public IQueryable<Venue> GetTopBookedVenues()
        {
            var topBookedVenues = _context.Venues
                .Include(v => v.Bookings)
                .Where(v => v.Bookings.Count > 5)
                .OrderByDescending(v => v.Bookings.Count)
                .Take(5)
                .ToList();

            return topBookedVenues.AsQueryable();
        }

        public Venue GetVenueById(int id)
        {
            return _context.Venues.Include(v=>v.Bookings).FirstOrDefault(v=>v.Id==id);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return _context.Categories.FirstOrDefault(c => c.Name == categoryName);
        }

        public void AddCategoryIfNotExist(Category category)
        {
            if (category.Id == 0)
            {
                _context.Categories.Add(category);
            }
        }

        public void AddVenue(Venue venue)
        {
            _context.Venues.Add(venue); 
            _context.SaveChanges();
        }

        public void AddSlot(Slot slot)
        {
            _context.Slots.Add(slot);
        }

        public void RemoveVenue(Venue venue)
        {
            _context.Venues.Remove(venue);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
