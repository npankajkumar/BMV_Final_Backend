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
        private readonly BmvContext _bmvContext;

        public NBookingService(IBookingRepository bookingRepository, BmvContext bmvContext)
        {
            _bookingRepository = bookingRepository;
            _bmvContext = bmvContext;
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
                Console.WriteLine("slots len<1");
                return null;
            }
            var slot1 = _bmvContext.Slots.Find(value.SlotIds[0]);
            if (slot1 == null)
            {
                Console.WriteLine("slot1 is null");
                return null;
            }
            var vId = slot1.VenueId;
            var venue = _bmvContext.Venues.Find(vId);
            if (venue == null)
            {
                Console.WriteLine("provider is null");
                return null;
            }
            Booking b = new Booking();
            b.CustomerId = customerId;
            b.VenueId = vId;
            b.ProviderId = venue.ProviderId;
            var today = DateTime.Now;
            var bDate = DateOnly.ParseExact(value.Date, "dd-MM-yyyy");
            if (bDate < DateOnly.FromDateTime(today))
            {
                Console.WriteLine("booking date < today");
                return null;
            }
            b.Date = bDate;
            _bmvContext.Bookings.Add(b);
            b.Start = TimeOnly.MaxValue;
            b.End = TimeOnly.MinValue;
            try
            {
                _bmvContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            double price = 0;
            foreach (var s in value.SlotIds)
            {
                var slot = _bmvContext.Slots.Find(s);
                if (slot != null)
                {
                    if (bDate == DateOnly.FromDateTime(today) && slot.End < TimeOnly.FromDateTime(today))
                    {
                        Console.WriteLine("116");
                        return null;
                    }
                    _bmvContext.BookedSlots.Add(new BookedSlot() { VenueId = vId, BookingId = b.Id, Date = b.Date, SlotId = s });
                    if (slot.Start < b.Start)
                    {
                        b.Start = slot.Start;
                    }
                    if (slot.End > b.End)
                    {
                        b.End = slot.End;
                    }
                    if ((int)bDate.DayOfWeek == 0 || (int)bDate.DayOfWeek == 6)
                    {
                        price += slot.WeekendPrice;
                    }
                    else
                    {
                        price += slot.WeekdayPrice;
                    }
                }
            }
            b.Amount = price + (price > 1000 ? 50 : 10);
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }
            return b;
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
