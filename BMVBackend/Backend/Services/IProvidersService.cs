using Backend.DTO.Provider;
using Backend.Models;

namespace Backend.Services
{
    public interface IProvidersService
    {
        bool AddProvider(Provider p);
        bool DeleteProvider(int id);
        List<Provider> GetAllProviders();
        Provider GetProviderById(int id);
        Provider GetProviderByEmail(string email);
        Provider RegisterProvider(ProviderRegisterDTO provider);
        Provider UpdateProvider(int id, Provider p);
        Provider ValidateProvider(ProviderLoginDTO provider);
    }
}