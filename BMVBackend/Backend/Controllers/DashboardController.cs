using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly BmvContext _bmvContext;
        
        //private readonly BmvContext _bmvContext = new BmvContext();
        private readonly INProvidersService _providersService;
        public DashboardController(INProvidersService providersService,BmvContext bmvContext) {
            _providersService = providersService;
            _bmvContext = bmvContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            Console.WriteLine("in dashboard");
            var providerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Provider p = _providersService.GetProviderByEmail(providerEmail);
            if (p == null)
            {
                return BadRequest();
            }
            var bookings = _bmvContext.Bookings.Where(b=>b.ProviderId== p.Id).ToList();
            var totalEarnings = bookings.Sum(b => b.Amount);
            var totalBookings = bookings.Count();
            var venues = _bmvContext.Venues.ToList();
            var ratingSum = venues.Sum(v => v.Rating);
            var overallRating = ratingSum/(venues.Count() < 1 ? 1: venues.Count());
            var recentBookings = bookings.OrderByDescending(b=>b.CreatedAt).Take(5);
            var chartData = new ChartData(bookings);
            var today = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine(overallRating.ToString());
            Console.WriteLine(chartData);

            List<RecentBookingDTO> recents = new List<RecentBookingDTO>();
            foreach (var item in recentBookings)
            {
                RecentBookingDTO r = new RecentBookingDTO();
                r.Id = item.Id;
                r.CreatedAt = item.CreatedAt;
                r.Amount = item.Amount;
                r.CustomerName = _bmvContext.Customers.Find(item.CustomerId).Name;
                recents.Add(r);
            }

            return Ok(new {TotalEarnings= totalEarnings, TotalBookings=totalBookings, OverallRating=overallRating, CData=chartData, RecentBookings=recents });
        }
    }
    public class DashboardDTO
    {
        public double TotalEarnings { get; set; }
        public double TotalBookings { get; set; }
        public float OverallRating { get; set; }
        public ChartData CData { get; set; }
    }
    public class ChartData
    {
        public int[] days { get; set; }
        public double[] earnings { get; set; }
        public ChartData(List<Booking> bookings)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            days = Enumerable.Range(1, daysInMonth).ToArray();
            earnings = new double[daysInMonth];

            foreach (var booking in bookings)
            {
                if (booking.CreatedAt.Month == currentMonth && booking.CreatedAt.Year == currentYear)
                {
                    var day = booking.CreatedAt.Day;
                    earnings[day - 1] += (double)booking.Amount;
                }
            }
        }
    }

    public class RecentBookingDTO()
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }

        public double Amount { get; set; }
    }
}
