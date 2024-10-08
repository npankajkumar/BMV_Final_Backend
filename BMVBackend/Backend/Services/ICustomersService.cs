using Backend.DTO.Customer;
using Backend.Models;

namespace Backend.Services
{
    public interface ICustomersService
    {
        bool AddCustomer(Customer u);
        bool DeleteCustomer(int id);
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string email);
        Customer RegisterCustomer(CustomerRegisterDTO customer);
        Customer UpdateCustomer(int id, Customer u);
        Customer ValidateCustomer(CustomerLoginDTO customer);
    }
}