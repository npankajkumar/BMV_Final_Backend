using Backend.DTO;
using Backend.DTO.Provider;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ProvidersService : IProvidersService
    {
        private readonly BmvContext _bmvContext;
        public ProvidersService(BmvContext bmvContext)
        {
            _bmvContext = bmvContext;
        }
        public List<Provider> GetAllProviders()
        {
            try
            {
                return _bmvContext.Providers.Include("Bookings").Include("Venues")
                    .ToList();
            }
            catch
            {
                return null;
            }
        }
        public Provider GetProviderById(int id)
        {
            var p = _bmvContext.Providers
    .Include(p => p.Bookings)
    .Include(p => p.Venues)
        .ThenInclude(v => v.Slots)
    .Where(p => p.Id == id)
    .FirstOrDefault();
            if (p == null)
            {
                Console.WriteLine("");
            }
            return p;
        }
        public Provider GetProviderByEmail(string email)
        {
            var p = _bmvContext.Providers
    .Include(p => p.Bookings)
    .Include(p => p.Venues)
        .ThenInclude(v => v.Slots)
    .Where(p => p.Email == email)
    .FirstOrDefault();
            if (p == null)
            {
                Console.WriteLine("");
            }
            return p;
        }
        public bool AddProvider(Provider p)
        {
            try
            {
                _bmvContext.Providers.Add(p);
                int x = _bmvContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Provider UpdateProvider(int id, Provider p)
        {
            Provider cProvider;
            try
            {
                cProvider = _bmvContext.Providers.Find(id);
                if (cProvider == null)
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            cProvider.Email = p.Email == null ? cProvider.Email : p.Email;
            cProvider.Mobile = p.Mobile == null ? cProvider.Mobile : p.Mobile;
            cProvider.Name = p.Name == null ? cProvider.Name : p.Name;
            try
            {
                _bmvContext.SaveChanges();
                return cProvider;
            }
            catch
            {
                return null;
            }
        }
        public bool DeleteProvider(int id)
        {
            var dp = _bmvContext.Providers.Find(id);
            if (dp != null)
            {
                _bmvContext.Providers.Remove(dp);
                _bmvContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public Provider ValidateProvider(ProviderLoginDTO provider)
        {
            if (provider == null)
            {
                return null;
            }
            var result = _bmvContext.Providers
        .FirstOrDefault(p => p.Email == provider.Email );

            return result;
        }
        public Provider RegisterProvider(ProviderRegisterDTO provider)
        {
            if (provider == null)
            {
                return null;
            }
            Provider p = new Provider() { Mobile = provider.Mobile, Email = provider.Email, Name = provider.Name };
            _bmvContext.Providers.Add(p);
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }
            return p;
        }
    }
}
