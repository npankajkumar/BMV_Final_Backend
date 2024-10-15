using Backend.Controllers;
using Backend.DTO.Customer;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BackendTesting
{
    [TestFixture]
    public class CustomerControllerTest
    {
        Mock<INCustomersService> service;
        CustomersController controller;
        Customer customer;
        CustomerLoginDTO loginDto;
        PutCustomerDTO customerDto;
        [OneTimeSetUp]
        public void SetUp() { 
            service = new Mock<INCustomersService>();
            controller = new CustomersController(service.Object);
            customer = new Customer { Id = 1, Name = "Niketh Donthula", Mobile = "1234567890", Email = "nikethdonthula@gmail.com" };
            loginDto = new CustomerLoginDTO
            {
                Email = "nikethdonthula@gmail.com",
                Password = "pass@1234"
            };
           customerDto = new PutCustomerDTO
            {
                Name = "Niketh",
                Mobile = "1234567890",
                Email = "niketh@gmail.com",
                Password = "pass@123"
            };
        }

        [Test]
        public void GetCustomerPositiveTest() {
            var customerId = "1";
            service.Setup(s => s.GetCustomerById(1)).Returns(customer);
            var userClaims = new List<Claim> { new Claim("CustomerId", customerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Get();
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetCustomerNegativeTest() {
            var customerId = "1";
            service.Setup(s => s.GetCustomerById(1)).Returns((Customer)null);
            var userClaims = new List<Claim> { new Claim("CustomerId", customerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Get();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void PostCustomerPositiveTest() {

            service.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(true);
            var result = controller.Post(new PostCustomerDTO
            {
                Email = "test@gmail.com",
                Name = "Test",
                Mobile = "1234567890",
                Password = "test@1234"
            });
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void PostCustomerNegativeTest() {
            service.Setup(s => s.AddCustomer(It.IsAny<Customer>())).Returns(false);
            var result = controller.Post(new PostCustomerDTO { });
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void PutCustomerPositiveTest()
        {
            var customerId = "1";
            var updatedCustomer = new Customer
            {
                Id = 1,
                Name = customerDto.Name,
                Mobile = customerDto.Mobile,
                Email = customerDto.Email,
               
            };
            service.Setup(s => s.UpdateCustomer(It.IsAny<int>(), It.IsAny<Customer>())).Returns(updatedCustomer);
            var userClaims = new List<Claim> { new Claim("CustomerId", customerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Put(customerDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public void PutCustomerNegativeTest() {
            var customerId = "1";
            var updatedCustomer = new Customer
            {
                Id = 1,
                Name = customerDto.Name,
                Mobile = customerDto.Mobile,
                Email = customerDto.Email,
                
            };
            service.Setup(s => s.UpdateCustomer(It.IsAny<int>(), It.IsAny<Customer>())).Returns((Customer)null);
            var userClaims = new List<Claim> { new Claim("CustomerId", customerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Put(customerDto);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


        [Test]
        public void ValidateCustomerPositiveTest()
        {
            service.Setup(s => s.ValidateCustomer(loginDto)).Returns(customer);
            var result = controller.Validate(loginDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ValidateCustomerNegativeTest()
        {
            var result = controller.Validate(null);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void ValidateCustomerNotFound()
        {
            service.Setup(s => s.ValidateCustomer(loginDto)).Returns((Customer)null);
            var result = controller.Validate(loginDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        
    }
}
