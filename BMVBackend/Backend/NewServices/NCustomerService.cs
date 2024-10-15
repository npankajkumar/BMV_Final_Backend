using Backend.DTO.Customer;
using Backend.Models;
using Backend.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class NCustomersService : INCustomersService
    {
        private readonly ICustomerRepository _customerRepository;

        public NCustomersService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public List<Customer> GetAllCustomers()
        {
            try
            {
                return _customerRepository.GetAllCustomers().ToList();
            }
            catch
            {
                return null;
            }
        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                return _customerRepository.GetCustomerById(id);
            }
            catch
            {
                return null;
            }
        }

        public Customer GetCustomerByEmail(string email)
        {
            try
            {
                return _customerRepository.GetCustomerByEmail(email);
            }
            catch
            {
                return null;
            }
        }

        public bool AddCustomer(Customer customer)
        {
            try
            {
                _customerRepository.AddCustomer(customer);
                _customerRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Customer UpdateCustomer(int id, Customer updatedCustomer)
        {
            var existingCustomer = _customerRepository.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return null;
            }

            existingCustomer.Email = updatedCustomer.Email ?? existingCustomer.Email;
            existingCustomer.Mobile = updatedCustomer.Mobile ?? existingCustomer.Mobile;
            existingCustomer.Name = updatedCustomer.Name ?? existingCustomer.Name;

            try
            {
                _customerRepository.SaveChanges();
                return existingCustomer;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return false;
            }

            _customerRepository.RemoveCustomer(customer);
            _customerRepository.SaveChanges();
            return true;
        }

        public Customer ValidateCustomer(CustomerLoginDTO customer)
        {
            if (customer == null)
            {
                return null;
            }

            return _customerRepository.ValidateCustomer(customer.Email);
        }

        public Customer RegisterCustomer(CustomerRegisterDTO customerDTO)
        {
            if (customerDTO == null)
            {
                return null;
            }

            Customer customer = new Customer
            {
                Mobile = customerDTO.Mobile,
                Email = customerDTO.Email,
                Name = customerDTO.Name
            };

            try
            {
                _customerRepository.AddCustomer(customer);
                _customerRepository.SaveChanges();
                return customer;
            }
            catch
            {
                return null;
            }
        }
    }
}
