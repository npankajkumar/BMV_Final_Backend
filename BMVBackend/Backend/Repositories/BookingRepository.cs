using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BmvContext _context;

        public BookingRepository(BmvContext context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetAllBookings()
        {
            return _context.Bookings.Include(b => b.BookedSlots).AsQueryable();
        }

        public IQueryable<Booking> GetBookingsByProviderId(int providerId)
        {
            return _context.Bookings.Include(b => b.BookedSlots).Where(b => b.ProviderId == providerId);
        }

        public IQueryable<Booking> GetBookingsByCustomerId(int customerId)
        {
            return _context.Bookings.Include(b => b.BookedSlots).Where(b => b.CustomerId == customerId);
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings.Find(id);
        }

        public Slot GetSlotById(int id)
        {
            return _context.Slots.Find(id);
        }

        public Venue GetVenueById(int id)
        {
            return _context.Venues.Find(id);
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void AddBookedSlot(BookedSlot bookedSlot)
        {
            _context.BookedSlots.Add(bookedSlot);
        }

        public void RemoveBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
