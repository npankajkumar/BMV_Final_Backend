using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Models;

namespace Backend.Services
{
    public interface IBookingService
    {
        Booking AddBooking(int customerId, BookingDTO value);
        bool DeleteBooking(int id);
        List<Booking> GetAllBookings();
        List<GetBookingDTO> GetAllBookingsByCustomerId(int id);
        List<GetBookingDTO> GetAllBookingsByProviderId(int id);
        Booking GetBookingById(int id);
        bool UpdateBooking(int id, Booking b);
    }
}