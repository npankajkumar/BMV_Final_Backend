using Backend.DTO.Customer;
using Backend.Models;

namespace Backend.Services
{
    public interface INCustomersService
    {
        bool AddCustomer(Customer customer);
        bool DeleteCustomer(int id);
        List<Customer> GetAllCustomers();
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerById(int id);
        Customer RegisterCustomer(CustomerRegisterDTO customerDTO);
        Customer UpdateCustomer(int id, Customer updatedCustomer);
        Customer ValidateCustomer(CustomerLoginDTO customer);
    }
}