using Backend.Controllers;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using static System.Net.WebRequestMethods;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            var logger = new LoggerConfiguration()
                .WriteTo.File("C:\\Users\\pulkit\\Desktop\\work\\Project\\backend\\BMVBackend\\Backend\\Logs\\Bookings.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Host.UseSerilog(logger);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowAnyOrigin();
                });
            });
            builder.Services.AddScoped<IProvidersService, ProvidersService>();
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddScoped<IVenuesService, VenuesService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<INProvidersService, NProvidersService>();
            builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
            builder.Services.AddScoped<INCustomersService, NCustomersService>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IVenueRepository,VenueRepository>();
            builder.Services.AddScoped<INVenuesService, NVenuesService>();
            builder.Services.AddScoped<INBookingService, NBookingService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INCategoryService, NCategoryService>();


            builder.Services.AddDbContext<BmvContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<AuthController>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
  options.Authority = $"https://{builder.Configuration["AuthSettings:TenantName"]}.b2clogin.com/{builder.Configuration["AuthSettings:TenantName"]}/v2.0/";
  options.TokenValidationParameters = new TokenValidationParameters
  {
      ValidateIssuer = true,
      ValidIssuer = $"https://{builder.Configuration["AuthSettings:TenantName"]}.b2clogin.com/{builder.Configuration["AuthSettings:TenantId"]}/v2.0/"
,
      ValidateAudience = true,
      ValidAudience = builder.Configuration["AuthSettings:ClientId"],
      ValidateLifetime = true,
      IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
      {
          // Retrieve the signing keys from the Azure AD B2C metadata endpoint
          var client = new HttpClient();
          var response = client.GetAsync("https://bookmyvenue.b2clogin.com/bookmyvenue.onmicrosoft.com/discovery/v2.0/keys?p=b2c_1_signupsignin2").Result;
          var keys = response.Content.ReadAsStringAsync().Result;
          return JsonWebKeySet.Create(keys).GetSigningKeys();
      }
  };
});

            var app = builder.Build();

            // Configure CORS
            app.UseCors("AllowAll");

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}