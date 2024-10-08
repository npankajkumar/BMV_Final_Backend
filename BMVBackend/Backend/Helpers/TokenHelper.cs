using Backend.DTO.Customer;
using Backend.DTO.Provider;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Backend.Helpers
{
    public class TokenHelper
    {
        private readonly IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public string GenerateCustomerToken(Customer cus)
        {
            string secret = _configuration["Jwt:secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[] {
                    new Claim("Role", "Customer"),
                    new Claim("Email",cus.Email),
                    new Claim("CustomerId", cus.Id.ToString() )
            };
            var token = new JwtSecurityToken(_configuration["Jwt:issuer"],
              _configuration["Jwt:audience"],
             claims,
             expires: DateTime.Now.AddMinutes(120),
             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public string GenerateProviderToken(Provider pro)
        {
            string secret = _configuration["Jwt:secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                    new Claim("Role", "Provider"),
                    new Claim("Email",pro.Email),
                    new Claim("ProviderId", pro.Id.ToString())

            };
            var token = new JwtSecurityToken(_configuration["Jwt:issuer"],
              _configuration["Jwt:audience"],
             claims,
             expires: DateTime.Now.AddMinutes(120),
             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

