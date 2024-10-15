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
            return _context.Venues.OrderByDescending(v => v.Rating).Take(5).AsQueryable();
        }

        public IQueryable<Venue> GetTopBookedVenues()
        {
            return _context.Venues.Include(v => v.Bookings).OrderByDescending(v => v.Bookings.Count).Take(5).AsQueryable();
        }

        public Venue GetVenueById(int id)
        {
            return _context.Venues.Find(id);
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
