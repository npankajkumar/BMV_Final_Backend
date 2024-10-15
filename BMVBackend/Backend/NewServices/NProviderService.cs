using Backend.DTO;
using Backend.DTO.Provider;
using Backend.Models;
using Backend.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class NProvidersService : INProvidersService
    {
        private readonly IProviderRepository _providerRepository;

        public NProvidersService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public List<Provider> GetAllProviders()
        {
            try
            {
                return _providerRepository.GetAllProviders().ToList();
            }
            catch
            {
                return null;
            }
        }

        public Provider GetProviderById(int id)
        {
            var provider = _providerRepository.GetProviderById(id);
            if (provider == null)
            {
                // Handle not found case
                Console.WriteLine("Provider not found");
            }
            return provider;
        }

        public Provider GetProviderByEmail(string email)
        {
            var provider = _providerRepository.GetProviderByEmail(email);
            if (provider == null)
            {
                // Handle not found case
                Console.WriteLine("Provider not found");
            }
            return provider;
        }

        public bool AddProvider(Provider provider)
        {
            try
            {
                _providerRepository.AddProvider(provider);
                _providerRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Provider UpdateProvider(int id, Provider updatedProvider)
        {
            var existingProvider = _providerRepository.GetProviderById(id);
            if (existingProvider == null)
            {
                return null;
            }

            existingProvider.Email = updatedProvider.Email ?? existingProvider.Email;
            existingProvider.Mobile = updatedProvider.Mobile ?? existingProvider.Mobile;
            existingProvider.Name = updatedProvider.Name ?? existingProvider.Name;

            try
            {
                _providerRepository.SaveChanges();
                return existingProvider;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteProvider(int id)
        {
            var provider = _providerRepository.GetProviderById(id);
            if (provider == null)
            {
                return false;
            }

            _providerRepository.RemoveProvider(provider);
            _providerRepository.SaveChanges();
            return true;
        }

        public Provider ValidateProvider(ProviderLoginDTO provider)
        {
            if (provider == null)
            {
                return null;
            }

            return _providerRepository.ValidateProvider(provider.Email);
        }

        public Provider RegisterProvider(ProviderRegisterDTO providerDTO)
        {
            if (providerDTO == null)
            {
                return null;
            }

            Provider provider = new Provider
            {
                Mobile = providerDTO.Mobile,
                Email = providerDTO.Email,
                Name = providerDTO.Name
            };

            try
            {
                _providerRepository.AddProvider(provider);
                _providerRepository.SaveChanges();
                return provider;
            }
            catch
            {
                return null;
            }
        }
    }
}