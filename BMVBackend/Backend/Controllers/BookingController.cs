using Backend.DTO;
using Backend.DTO.Booking;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        INBookingService _service;
        INCustomersService _customerService;
        INVenuesService _venueService;
        INProvidersService _providerService;
        ILogger _logger;
        public BookingController(INBookingService bookingService, INCustomersService customerService, INVenuesService venueService, INProvidersService providerService, ILogger<BookingController> logger)
        {
            _service = bookingService;
            _customerService = customerService;
            _venueService = venueService;
            _providerService = providerService;
            _logger = logger;
        }
        // GET: api/<BookingController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            _logger.LogInformation("Booking Log Information");
            var customerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            var providerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            var headerValue = Request.Headers["User"].ToString();
            List<GetBookingDTO> bookings;
            if (headerValue == "provider")
            {
                Provider p = _providerService.GetProviderByEmail(providerEmail);
                bookings = _service.GetAllBookingsByProviderId(p.Id);
            }
            else
            {
                Customer c = _customerService.GetCustomerByEmail(customerEmail);
                bookings = _service.GetAllBookingsByCustomerId(c.Id);
            }
            var detailedBookings = bookings.Select(b => new
            {
                b.Id,
                b.CreatedAt,
                b.Status,
                b.CustomerId,
                b.ProviderId,
                b.VenueId,
                b.Amount,
                b.Date,
                b.Start,
                b.End,
                b.BookedSlots,
                CustomerName = _customerService.GetCustomerById(b.CustomerId)?.Name,
                ProviderName = _providerService.GetProviderById(b.ProviderId)?.Name,
                CustomerMobile = _customerService.GetCustomerById(b.CustomerId)?.Mobile,
                ProviderMobile = _providerService.GetProviderById(b.ProviderId)?.Mobile,
                CustomerEmail = _customerService.GetCustomerById(b.CustomerId)?.Email,
                ProviderEmail = _providerService.GetProviderById(b.ProviderId)?.Email,
                VenueName = _venueService.GetVenueById(b.VenueId)?.Name
            }).ToList();

            return Ok(detailedBookings);
        }

        
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] BookingDTO value)
        {

            var customerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Customer c = _customerService.GetCustomerByEmail(customerEmail);
            if (c == null)
            {
                Console.WriteLine("null customer");
                return BadRequest();
            }

            var b = _service.AddBooking(c.Id, value);
            if (b == null)
            {
                return BadRequest();
            }
            return Ok(b);

        }
    }
}
