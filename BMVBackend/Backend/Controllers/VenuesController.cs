
using Backend.DTO.Venue;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        INVenuesService _service;
        INProvidersService _providersService;
        private readonly BmvContext _bmvContext;
        public VenuesController(INVenuesService service,INProvidersService providersService, BmvContext bmvContext)
        {
            _service = service;
            _providersService = providersService;
            _bmvContext = bmvContext;
        }

        // GET: api/<VenueController>
        
        [HttpGet]
        public IActionResult Get([FromQuery] bool? topRated, [FromQuery] bool? topBooked)
        {
            User.Claims.ToList().ForEach(c=> Console.WriteLine(c));
            var res = new GetVenuesDTO();
            if (topRated!= null && topRated == true)
            {
                var x = _service.GetTopRatedVenues();

                res.TopRatedVenues = x;
            }
            if(topBooked != null && topBooked == true)
            {
                res.TopBookedVenues = _service.GetTopBookedVenues();
            }
            if((topRated != null && topRated == true) || (topBooked != null && topBooked == true))
            {
                return Ok(res);
            }
            var v = _service.GetAllVenues();
            if (v == null) {
                return BadRequest();
            }
            
            return Ok(v);
        }

        // GET api/<VenueController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var v = _service.GetVenueById(id);
            if(v == null)
            {
                return NotFound();
            }
            var cservice = new CategoryService(_bmvContext);
            var cName = cservice.GetCategoryById(v.CategoryId).Name;
            return Ok(v);
        }

        // POST api/<VenueController>
        [HttpPost]
        [Authorize]
        public IActionResult Post(PostVenueDTO venueWithSlotDetails)
        {
            var providerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Provider p = _providersService.GetProviderByEmail(providerEmail);
            if (p == null)
            {
                return BadRequest();

            }
            var v = _service.AddVenue(p.Id,venueWithSlotDetails);
            return Ok(v);
        }

        // PUT api/<VenueController>/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] PutVenueDTO v)
        {
            var providerEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            Provider p = _providersService.GetProviderByEmail(providerEmail);
            if (p == null)
            {
                return BadRequest("Could not retrieve provider email from token");
            }
            //var cv = _service.GetVenueById(id);
            //if (cv == null || cv.ProviderId != p.Id) { 
            //    return BadRequest();
            //}
            var uv = _service.UpdateVenue(id, v);
            if (uv != null)
            {
                return Ok(uv);
            }
            return NotFound("Venue Not Found");
        }

        // DELETE api/<VenueController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
