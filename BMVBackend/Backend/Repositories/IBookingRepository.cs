using Backend.Models;

namespace Backend.Repositories
{
    public interface IBookingRepository
    {
        void AddBookedSlot(BookedSlot bookedSlot);
        void AddBooking(Booking booking);
        IQueryable<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        IQueryable<Booking> GetBookingsByCustomerId(int customerId);
        IQueryable<Booking> GetBookingsByProviderId(int providerId);
        Slot GetSlotById(int id);
        Venue GetVenueById(int id);
        void RemoveBooking(Booking booking);
        void SaveChanges();
    }
}