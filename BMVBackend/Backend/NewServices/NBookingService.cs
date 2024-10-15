using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Models;
using Backend.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class NBookingService : INBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public NBookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings().ToList();
        }

        public List<GetBookingDTO> GetAllBookingsByProviderId(int providerId)
        {
            var bookings = _bookingRepository.GetBookingsByProviderId(providerId).ToList();
            return bookings.Select(b => new GetBookingDTO
            {
                Id = b.Id,
                ProviderId = b.ProviderId,
                VenueId = b.VenueId,
                CustomerId = b.CustomerId,
                Amount = b.Amount,
                Date = b.Date,
                Start = b.Start,
                End = b.End,
                BookedSlots = b.BookedSlots,
                Status = b.End > TimeOnly.FromDateTime(System.DateTime.Now) ? "done" : "upcoming"
            }).ToList();
        }

        public List<GetBookingDTO> GetAllBookingsByCustomerId(int customerId)
        {
            var bookings = _bookingRepository.GetBookingsByCustomerId(customerId).ToList();
            return bookings.Select(b => new GetBookingDTO
            {
                Id = b.Id,
                ProviderId = b.ProviderId,
                VenueId = b.VenueId,
                CustomerId = b.CustomerId,
                Amount = b.Amount,
                Date = b.Date,
                Start = b.Start,
                End = b.End,
                BookedSlots = b.BookedSlots,
                Status = b.End > TimeOnly.FromDateTime(System.DateTime.Now) ? "done" : "upcoming"
            }).ToList();
        }

        public Booking GetBookingById(int id)
        {
            return _bookingRepository.GetBookingById(id);
        }

        public Booking AddBooking(int customerId, BookingDTO value)
        {
            if (value.SlotIds.Length < 1)
            {
                return null;
            }

            var slot1 = _bookingRepository.GetSlotById(value.SlotIds[0]);
            if (slot1 == null)
            {
                return null;
            }

            var venue = _bookingRepository.GetVenueById(slot1.VenueId);
            if (venue == null)
            {
                return null;
            }

            var bookingDate = DateOnly.ParseExact(value.Date, "dd-MM-yyyy");
            if (bookingDate < DateOnly.FromDateTime(System.DateTime.Now))
            {
                return null;
            }

            var booking = new Booking
            {
                CustomerId = customerId,
                VenueId = slot1.VenueId,
                ProviderId = venue.ProviderId,
                Date = bookingDate,
                Start = TimeOnly.MaxValue,
                End = TimeOnly.MinValue
            };

            _bookingRepository.AddBooking(booking);
            double price = 0;

            foreach (var slotId in value.SlotIds)
            {
                var slot = _bookingRepository.GetSlotById(slotId);
                if (slot != null)
                {
                    _bookingRepository.AddBookedSlot(new BookedSlot
                    {
                        VenueId = slot1.VenueId,
                        BookingId = booking.Id,
                        Date = booking.Date,
                        SlotId = slotId
                    });

                    if (slot.Start < booking.Start)
                    {
                        booking.Start = slot.Start;
                    }

                    if (slot.End > booking.End)
                    {
                        booking.End = slot.End;
                    }

                    price += (booking.Date.DayOfWeek == DayOfWeek.Saturday || booking.Date.DayOfWeek == DayOfWeek.Sunday)
                        ? slot.WeekendPrice
                        : slot.WeekdayPrice;
                }
            }

            booking.Amount = price + (price > 1000 ? 50 : 10);
            _bookingRepository.SaveChanges();

            return booking;
        }

        public bool UpdateBooking(int id, Booking updatedBooking)
        {
            var existingBooking = _bookingRepository.GetBookingById(id);
            if (existingBooking != null)
            {
                existingBooking.CreatedAt = updatedBooking.CreatedAt;
                existingBooking.End = updatedBooking.End;
                existingBooking.Date = updatedBooking.Date;
                existingBooking.Start = updatedBooking.Start;
                existingBooking.Amount = updatedBooking.Amount;
                _bookingRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteBooking(int id)
        {
            var booking = _bookingRepository.GetBookingById(id);
            if (booking != null)
            {
                _bookingRepository.RemoveBooking(booking);
                _bookingRepository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
