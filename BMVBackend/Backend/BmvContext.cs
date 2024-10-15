using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Backend
{
    public class BmvContext : DbContext
    {
        public BmvContext(DbContextOptions<BmvContext> options) : base(options) { }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedSlot> BookedSlots { get; set; }
        public DbSet<Category> Categories { get; set; }
        
    }
}
