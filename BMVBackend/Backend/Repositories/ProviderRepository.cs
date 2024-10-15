//using Backend.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Backend.Repositories
//{
//    public class ProviderRepository : IProviderRepository
//    {
//        private readonly BmvContext _context;

//        public ProviderRepository(BmvContext context)
//        {
//            _context = context;
//        }

//        public IQueryable<Provider> GetAll()
//        {
//            return _context.Providers.Include("Bookings").Include("Venues").AsQueryable();
//        }

//        public Provider GetById(int id)
//        {
//            return _context.Providers
//                .Include(p => p.Bookings)
//                .Include(p => p.Venues)
//                .FirstOrDefault(p => p.Id == id);
//        }

//        public Provider GetByEmail(string email)
//        {
//            return _context.Providers
//                .Include(p => p.Bookings)
//                .Include(p => p.Venues)
//                .FirstOrDefault(p => p.Email == email);
//        }

//        public void Add(Provider provider)
//        {
//            _context.Providers.Add(provider);
//        }

//        public void Remove(Provider provider)
//        {
//            _context.Providers.Remove(provider);
//        }

//        public void SaveChanges()
//        {
//            _context.SaveChanges();
//        }
//    }
//}

using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly BmvContext _context;

        public ProviderRepository(BmvContext context)
        {
            _context = context;
        }

        public IQueryable<Provider> GetAllProviders()
        {
            return _context.Providers
                .Include(p => p.Bookings)
                .Include(p => p.Venues)
                    .ThenInclude(v => v.Slots)
                .AsQueryable();
        }

        public Provider GetProviderById(int id)
        {
            return _context.Providers
                .Include(p => p.Bookings)
                .Include(p => p.Venues)
                    .ThenInclude(v => v.Slots)
                .FirstOrDefault(p => p.Id == id);
        }

        public Provider GetProviderByEmail(string email)
        {
            return _context.Providers
                .Include(p => p.Bookings)
                .Include(p => p.Venues)
                    .ThenInclude(v => v.Slots)
                .FirstOrDefault(p => p.Email == email);
        }

        public void AddProvider(Provider provider)
        {
            _context.Providers.Add(provider);
        }

        public void RemoveProvider(Provider provider)
        {
            _context.Providers.Remove(provider);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Provider ValidateProvider(string email)
        {
            return _context.Providers.FirstOrDefault(p => p.Email == email);
        }
    }
}
