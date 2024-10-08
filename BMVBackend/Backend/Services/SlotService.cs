using Backend.DTO;
using Backend.DTO.Slot;
using Backend.Models;

namespace Backend.Services
{
    public class SlotService
    {
        private readonly BmvContext _bmvContext = new BmvContext();
        public List<Slot> GetAllSlots()
        {
            return _bmvContext.Slots.ToList();
        }
        public List<GetSlotDTO> GetAllSlots(int venueId, DateOnly date)
        {
            var today = DateTime.Now;
            List<GetSlotDTO> availableSlots = new List<GetSlotDTO>();
            if (date<DateOnly.FromDateTime(today))
            {
                return availableSlots;
            }
            var allSlots = _bmvContext.Slots.Where(s=>s.VenueId==venueId).ToList();
            var bookedSlots = _bmvContext.BookedSlots.Where(bs=>bs.Date==date).ToList();
            foreach (var slot in allSlots)
            {
                if(date == DateOnly.FromDateTime(today) && slot.End < TimeOnly.FromDateTime(today))
                {
                    continue;
                }
                GetSlotDTO newSlot = new GetSlotDTO();
                newSlot.Id = slot.Id;
                newSlot.Start = slot.Start;
                newSlot.End = slot.End;
                newSlot.WeekendPrice = slot.WeekendPrice;
                newSlot.WeekdayPrice = slot.WeekdayPrice;
                newSlot.Status = "available";
                if (slot.IsBlocked)
                {
                    newSlot.Status = "blocked";
                    availableSlots.Add(newSlot);
                    continue;
                }
                foreach (var bs in bookedSlots)
                {
                    
                    if(bs.SlotId == slot.Id)
                    {
                        newSlot.Status = "booked";
                    }
                }
                availableSlots.Add(newSlot);
            }
            return availableSlots;
        }
        public Slot GetSlotById(int id)
        {
            return _bmvContext.Slots.Find(id);
        }
        public bool AddSlot(Slot s)
        {
            try
            {
                _bmvContext.Slots.Add(s);
                _bmvContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Slot UpdateSlot(int id,PutSlotDTO s)
        {
            var us = _bmvContext.Slots.Find(id);
            if (us == null)
            {
                return null;
            }
            us.WeekendPrice = s.WeekendPrice == null? us.WeekendPrice : (double)s.WeekendPrice;
            us.WeekdayPrice = s.WeekdayPrice == null ? us.WeekdayPrice : (double)s.WeekdayPrice;
            us.IsBlocked = s.IsBlocked == null ? us.IsBlocked : (bool)s.IsBlocked;
            _bmvContext.SaveChanges();
            return us;
        }
        public bool DeleteSlot(int id)
        {
            var ds = _bmvContext.Slots.Find(id);
            if (ds != null)
            {
                _bmvContext.Slots.Remove(ds);
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
      
    }
}
