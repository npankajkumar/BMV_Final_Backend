using Backend.DTO.Customer;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly BmvContext _bmvContext;
        public CustomersService(BmvContext bmvContext)
        {
            _bmvContext = bmvContext;
        }
        public List<Customer> GetAllCustomers()
        {
            List<Customer> users;
            try
            {
                users = _bmvContext.Customers.Include("Bookings").ToList();
                return users;
            }
            catch
            {
                return null;
            }
        }
        public Customer GetCustomerById(int id)
        {
            Customer user;
            try
            {
                user = _bmvContext.Customers.FirstOrDefault(x => x.Id == id);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public Customer GetCustomerByEmail(string email)
        {
            Customer user;
            try
            {
                user = _bmvContext.Customers.FirstOrDefault(x => x.Email == email);
                return user;
            }
            catch
            {
                return null;
            }
        }
        public bool AddCustomer(Customer u)
        {
            try
            {
                _bmvContext.Customers.Add(u);
                _bmvContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Customer UpdateCustomer(int id, Customer c)
        {
            Customer cCustomer;
            try
            {
                cCustomer = _bmvContext.Customers.Find(id);
                if (cCustomer == null)
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            cCustomer.Email = c.Email == null ? cCustomer.Email : c.Email;
            cCustomer.Mobile = c.Mobile == null ? cCustomer.Mobile : c.Mobile;
            cCustomer.Name = c.Name == null ? cCustomer.Name : c.Name;
            try
            {
                _bmvContext.SaveChanges();
                return cCustomer;
            }
            catch
            {
                return null;
            }
        }
        public bool DeleteCustomer(int id)
        {
            var du = _bmvContext.Customers.Find(id);
            if (du != null)
            {
                _bmvContext.Customers.Remove(du);
                _bmvContext.SaveChanges();
                return true;
            }
            return false;
        }
        public Customer ValidateCustomer(CustomerLoginDTO customer)
        {
            if (customer == null)
            {
                return null;
            }
            var result = _bmvContext.Customers
        .FirstOrDefault(u => u.Email == customer.Email );

            return result;
        }
        public Customer RegisterCustomer(CustomerRegisterDTO customer)
        {
            if (customer == null)
            {
                return null;
            }
            Customer c = new Customer() { Mobile = customer.Mobile, Email = customer.Email, Name = customer.Name };
            _bmvContext.Customers.Add(c);
            try
            {
                _bmvContext.SaveChanges();
            }
            catch
            {
                return null;
            }
            return c;
        }

    }
}
