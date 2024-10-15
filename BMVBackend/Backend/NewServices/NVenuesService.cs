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

        public NVenuesService(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
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

            Venue venue = new Venue
            {
                Name = venueWithSlotDetails.Name,
                Description = venueWithSlotDetails.Description,
                Address = venueWithSlotDetails.Address,
                City = venueWithSlotDetails.City,
                Latitude = venueWithSlotDetails.Latitude,
                Longitude = venueWithSlotDetails.Longitude,
                ProviderId = providerId
            };

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
            var category = _venueRepository.GetCategoryByName(venueWithSlotDetails.Category) ?? new Category { Name = venueWithSlotDetails.Category };
            _venueRepository.AddCategoryIfNotExist(category);

            venue.CategoryId = category.Id;
            _venueRepository.AddVenue(venue);

            foreach (var slot in slots)
            {
                _venueRepository.AddSlot(new Slot
                {
                    Start = slot.Start,
                    End = slot.End,
                    VenueId = venue.Id,
                    WeekdayPrice = slot.WeekdayPrice,
                    WeekendPrice = slot.WeekendPrice
                });
            }

            _venueRepository.SaveChanges();
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
                venue.Rating = venueDTO.Rating ?? venue.Rating;
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
