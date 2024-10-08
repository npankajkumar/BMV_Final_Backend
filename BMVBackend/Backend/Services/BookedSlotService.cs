using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Services
{
    public class BookedSlotService
    {
        private readonly BmvContext _bmvContext = new BmvContext();
        public List<BookedSlot> GetAllBookedSlots()
        {
            return _bmvContext.BookedSlots.ToList();
        }
        public BookedSlot GetBookedSlotById(int id)
        {
            return _bmvContext.BookedSlots.Find(id);   
        }
        public bool AddBookedSlot(BookedSlot bs)
        {
            try 
            {
                _bmvContext.BookedSlots.Add(bs);
                _bmvContext.SaveChanges();
                return true; 
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateBookedSlot(int id,BookedSlot bs)
        {
            var ubs = _bmvContext.BookedSlots.Find(id);
            if (ubs != null)
            {
                ubs.Date = bs.Date;
                ubs.BookingId = bs.BookingId;
                ubs.VenueId = bs.VenueId;
                ubs.SlotId = bs.SlotId;
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteBookedSlot(int id)
        {
            var dbs = _bmvContext.BookedSlots.Find(id);
            if (dbs != null)
            {
                _bmvContext.BookedSlots.Remove(dbs);
                _bmvContext.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
