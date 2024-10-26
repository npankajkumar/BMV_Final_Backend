using Backend.DTO.Venue;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class NVenuesService : INVenuesService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly BmvContext _bmvContext;
        public NVenuesService(IVenueRepository venueRepository, BmvContext bmvContext)
        {
            _venueRepository = venueRepository;
            _bmvContext = bmvContext;
        }

        public List<Venue> GetAllVenues()
        {
            return _venueRepository.GetAllVenues().ToList();
        }

        public List<Venue> GetTopRatedVenues()
        {
            return _venueRepository.GetTopRatedVenues().ToList();
        }

        public List<Venue> GetTopBookedVenues()
        {
            return _venueRepository.GetTopBookedVenues().ToList();
        }

        public Venue GetVenueById(int id)
        {
            return _venueRepository.GetVenueById(id);
        }

        public Venue AddVenue(int providerId, PostVenueDTO venueWithSlotDetails)
        {
            if (venueWithSlotDetails.slotDetails.DurationInMinutes < 15)
            {
                return null;
            }

            List<SlotDetails> slots = new List<SlotDetails>();
            var openingTime = TimeOnly.Parse(venueWithSlotDetails.slotDetails.OpeningTime);
            var closingTime = TimeOnly.Parse(venueWithSlotDetails.slotDetails.ClosingTime);
            var currentTime = openingTime;

            while (currentTime.AddMinutes(venueWithSlotDetails.slotDetails.DurationInMinutes - 1) <= closingTime)
            {
                TimeOnly endTime = currentTime.AddMinutes(venueWithSlotDetails.slotDetails.DurationInMinutes - 1);
                var newSlot = new SlotDetails()
                {
                    Start = currentTime,
                    End = endTime,
                    WeekdayPrice = venueWithSlotDetails.slotDetails.WeekdayPrice,
                    WeekendPrice = venueWithSlotDetails.slotDetails.WeekendPrice
                };

                slots.Add(newSlot);
                currentTime = endTime.AddMinutes(1);
            }
            Venue venue = new Venue();

            venue.Name = venueWithSlotDetails.Name;
            venue.Description = venueWithSlotDetails.Description;
            venue.Address = venueWithSlotDetails.Address;
            venue.City = venueWithSlotDetails.City;
            venue.Latitude = venueWithSlotDetails.Latitude;
            venue.Longitude = venueWithSlotDetails.Longitude;
            venue.ProviderId = providerId;

            // Image processing and category logic
            List<string> images = new List<string>();
            string path = "../wwwroot/images/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            ImageHelper imageHelper = new ImageHelper();
            foreach (IFormFile files in venueWithSlotDetails.Images)
            {
                if (files != null && files.Length > 0)
                {
                    images.Add(imageHelper.storeImage(files));
                }
            }

            venue.Images = images;
            Category c = _bmvContext.Categories.Where(c => c.Name == venueWithSlotDetails.Category).FirstOrDefault();
            if (c == null)
            {
                c = new Category() { Name = venueWithSlotDetails.Category };
                _bmvContext.Categories.Add(c);
            }
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }
            venue.CategoryId = c.Id;
            _bmvContext.Venues.Add(venue);
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }
            foreach (var slot in slots)
            {
                _bmvContext.Slots.Add(new Slot() { Start = slot.Start, End = slot.End, VenueId = venue.Id, WeekdayPrice = slot.WeekdayPrice, WeekendPrice = slot.WeekendPrice });
            }
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }

            return venue;
        }

        public Venue UpdateVenue(int id, PutVenueDTO venueDTO)
        {
            var venue = _venueRepository.GetVenueById(id);
            if (venue != null)
            {
                venue.City = venueDTO.City ?? venue.City;
                venue.Address = venueDTO.Address ?? venue.Address;
                venue.Description = venueDTO.Description ?? venue.Description;
                venue.Name = venueDTO.Name ?? venue.Name;
                if (venueDTO.Rating.HasValue)
                {
                    venue.RatingSum += venueDTO.Rating.Value;
                    venue.RatingCount += 1;
                }
                venue.Latitude = venueDTO.Latitude ?? venue.Latitude;
                venue.Longitude = venueDTO.Longitude ?? venue.Longitude;

                _venueRepository.SaveChanges();
                return venue;
            }
            return null;
        }

        public bool DeleteVenue(int id)
        {
            var venue = _venueRepository.GetVenueById(id);
            if (venue != null)
            {
                _venueRepository.RemoveVenue(venue);
                _venueRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }

    
}
