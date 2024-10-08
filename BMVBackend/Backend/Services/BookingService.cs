using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class BookingService : IBookingService
    {
        private readonly BmvContext _bmvContext = new BmvContext();
        public List<Booking> GetAllBookings()
        {
            return _bmvContext.Bookings.Include("BookedSlots").ToList();
        }
        public List<GetBookingDTO> GetAllBookingsByProviderId(int id)
        {
            List<GetBookingDTO> bookings = new List<GetBookingDTO>();
            var pBookings = _bmvContext.Bookings.Include("BookedSlots").Where(p => p.ProviderId == id).ToList();
            foreach (var item in pBookings)
            {
                var gBooking = new GetBookingDTO();
                gBooking.Start = item.Start;
                gBooking.CreatedAt = item.CreatedAt;
                gBooking.End = item.End;
                gBooking.Id = item.Id;
                gBooking.CustomerId = item.CustomerId;
                gBooking.ProviderId = item.ProviderId;
                gBooking.VenueId = item.VenueId;
                gBooking.Amount = item.Amount;
                gBooking.Date = item.Date;
                gBooking.BookedSlots = item.BookedSlots;
                gBooking.Status = item.End > TimeOnly.FromDateTime(DateTime.Now) ? "done" : "upcoming";
                bookings.Add(gBooking);
            }
            return bookings;
        }
        public List<GetBookingDTO> GetAllBookingsByCustomerId(int id)
        {
            List<GetBookingDTO> bookings = new List<GetBookingDTO>();
            var pBookings = _bmvContext.Bookings.Include("BookedSlots").Where(p => p.CustomerId == id).ToList();
            foreach (var item in pBookings)
            {
                var gBooking = new GetBookingDTO();
                gBooking.Start = item.Start;
                gBooking.CreatedAt = item.CreatedAt;
                gBooking.End = item.End;
                gBooking.Id = item.Id;
                gBooking.CustomerId = item.CustomerId;
                gBooking.ProviderId = item.ProviderId;
                gBooking.VenueId = item.VenueId;
                gBooking.Amount = item.Amount;
                gBooking.Date = item.Date;
                gBooking.BookedSlots = item.BookedSlots;
                gBooking.Status = item.End > TimeOnly.FromDateTime(DateTime.Now) ? "done" : "upcoming";
                bookings.Add(gBooking);
            }
            return bookings;
        }
        public Booking GetBookingById(int id)
        {
            return _bmvContext.Bookings.Find(id);
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
            if(venue == null)
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
            if(bDate < DateOnly.FromDateTime(today))
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
            catch(Exception e)
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
                    if(bDate == DateOnly.FromDateTime(today) && slot.End < TimeOnly.FromDateTime(today))
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
                Console.WriteLine("116");
                return null;
            }
            Console.WriteLine("148");
            return b;
        }
        public bool UpdateBooking(int id, Booking b)
        {
            var ub = _bmvContext.Bookings.Find(id);
            if (ub != null)
            {
                ub.CreatedAt = b.CreatedAt;
                ub.End = b.End;
                ub.Date = b.Date;
                ub.Start = b.Start;
                ub.Amount = b.Amount;
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteBooking(int id)
        {
            var db = _bmvContext.Bookings.Find(id);
            if (db != null)
            {
                _bmvContext.Bookings.Remove(db);
                _bmvContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
