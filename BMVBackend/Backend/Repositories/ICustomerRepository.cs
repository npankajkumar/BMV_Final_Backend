using Backend.Models;

namespace Backend.Repositories
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        IQueryable<Customer> GetAllCustomers();
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerById(int id);
        void RemoveCustomer(Customer customer);
        void SaveChanges();
        Customer ValidateCustomer(string email);
    }
}