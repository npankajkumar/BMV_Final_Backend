using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Models;

namespace Backend.Services
{
    public interface INBookingService
    {
        Booking AddBooking(int customerId, BookingDTO value);
        bool DeleteBooking(int id);
        List<Booking> GetAllBookings();
        List<GetBookingDTO> GetAllBookingsByCustomerId(int customerId);
        List<GetBookingDTO> GetAllBookingsByProviderId(int providerId);
        Booking GetBookingById(int id);
        bool UpdateBooking(int id, Booking updatedBooking);
    }
}