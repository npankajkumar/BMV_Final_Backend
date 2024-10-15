using Backend.DTO.Provider;
using Backend.Models;

namespace Backend.Services
{
    public interface INProvidersService
    {
        bool AddProvider(Provider provider);
        bool DeleteProvider(int id);
        List<Provider> GetAllProviders();
        Provider GetProviderByEmail(string email);
        Provider GetProviderById(int id);
        Provider RegisterProvider(ProviderRegisterDTO providerDTO);
        Provider UpdateProvider(int id, Provider updatedProvider);
        Provider ValidateProvider(ProviderLoginDTO provider);
    }
}