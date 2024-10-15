using Backend.DTO.Customer;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        INCustomersService _service;
        public CustomersController(INCustomersService usersService) 
        {
            _service = usersService;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var customerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Customer c = _service.GetCustomerByEmail(customerEmail);
            var customer = _service.GetCustomerById(c.Id);
            if (customer == null)
            {
                return BadRequest();
            }
            return Ok(new { Id = customer.Id, Name = customer.Name, Mobile = customer.Mobile, Email=customer.Email });

        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.GetCustomerById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] PostCustomerDTO user)
        {
            Customer newUser = new Customer();
            newUser.Mobile = user.Mobile;
            newUser.Name = user.Name;
            newUser.Email = user.Email;
          
            if (_service.AddCustomer(newUser))
            {
                return Ok(newUser);
            }
            return BadRequest();
        }

        // PUT api/<UserController>/5
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] PutCustomerDTO newUser)
        {

            var customerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Customer c = _service.GetCustomerByEmail(customerEmail);
            Customer user = new Customer() { Email = newUser.Email, Mobile = newUser.Mobile, Name = newUser.Name, };
            var updatedUser = _service.UpdateCustomer(c.Id, user);
            if (updatedUser == null)
            {
                return BadRequest();
            }
            return Ok(updatedUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Validate(CustomerLoginDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var cus = _service.ValidateCustomer(value);
            if (cus!=null)
            {

                return Ok(new TokenResult()
                {
                    Status = "success",
                    Token = new TokenHelper().GenerateCustomerToken(cus)
                });
            }
            return NotFound(new TokenResult() { Status = "failed", Token = null });
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(CustomerRegisterDTO value)
        {
            var c =_service.RegisterCustomer(value);
            if (c != null)
            {
                return Ok(new TokenResult()
                {
                    Status = "success",
                    Token = new TokenHelper().GenerateCustomerToken(c)
                });
            }
            return BadRequest(new TokenResult() { Status = "failed", Token = null });
        }
    }
}
