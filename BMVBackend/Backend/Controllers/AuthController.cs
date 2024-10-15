using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly INProvidersService _providerService;
        private readonly INCustomersService _customersService;
        private readonly IConfiguration _configuration;

        public AuthController(HttpClient httpClient,  INProvidersService providerService, INCustomersService customersService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _providerService = providerService;
            _customersService = customersService;
            _configuration = configuration;
        }

        [HttpGet("{Code}")]
        public async Task<IActionResult> Get(string Code)
        {
            string tokenUrl = "https://bookmyvenue.b2clogin.com/bookmyvenue.onmicrosoft.com/oauth2/v2.0/token?p=b2c_1_signupsignin2";

            var requestData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", _configuration["Secrets:GrantType"]),
                new KeyValuePair<string, string>("code", Code),
                new KeyValuePair<string, string>("redirect_uri", "http://localhost:5173/"),
                new KeyValuePair<string, string>("client_id", _configuration["Secrets:ClientId"]),
                new KeyValuePair<string, string>("client_secret",_configuration["Secrets:ClientSecret"] )
            });

            var response = await _httpClient.PostAsync(tokenUrl, requestData);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                if (responseData != null && responseData.ContainsKey("id_token"))
                {
                    string idToken = responseData["id_token"];
                    var token = idToken;
      
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(idToken);
                    var tokenS = jsonToken as JwtSecurityToken;
                    var newUser = tokenS.Claims.FirstOrDefault(claim => claim.Type == "newUser");

                        tokenS.Claims.ToList().ForEach(c=>Console.WriteLine(c));
                    if (newUser == null)
                        return Ok(new { id_token = idToken });
                    else
                    {
                        
                        var Name = tokenS.Claims.First(claim => claim.Type == "name").Value;
                        var Mobile = tokenS.Claims.First(claim => claim.Type == "extension_Mobile").Value;
                        var Email = tokenS.Claims.First(claim => claim.Type == "emails").Value;
                        Provider p = new Provider() { Email = Email, Mobile = Mobile, Name = Name };
                        Customer c = new Customer() { Email = Email, Mobile = Mobile, Name = Name };
                        _customersService.AddCustomer(c);
                        _providerService.AddProvider(p);
                        
                    }
                    return Ok(new {id_token = idToken });
                }
                return Ok(content); 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent); 
            }
        }

      
    }
}
