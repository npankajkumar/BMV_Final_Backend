//using Backend.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

using Backend.Models;

namespace Backend.Repositories
{
    public interface IProviderRepository
    {
        void AddProvider(Provider provider);
        IQueryable<Provider> GetAllProviders();
        Provider GetProviderByEmail(string email);
        Provider GetProviderById(int id);
        void RemoveProvider(Provider provider);
        void SaveChanges();
        Provider ValidateProvider(string email);
    }
}