using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BmvContext _context;

        public CustomerRepository(BmvContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            return _context.Customers.Include("Bookings").AsQueryable();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void RemoveCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Customer ValidateCustomer(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }
    }
}
