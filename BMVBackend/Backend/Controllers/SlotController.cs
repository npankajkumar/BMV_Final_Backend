using Backend.DTO.Slot;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        SlotService _service;
        public SlotController()
        {
            _service = new SlotService();
        }
        // GET: api/<SlotController>
        [HttpGet]
        public IActionResult Get([FromQuery] string date, [FromQuery] int venueId)
        {
            if (date == null || date == "" || venueId == 0)
            {
                return BadRequest();
            }
            var d = DateOnly.ParseExact(date, "dd-MM-yyyy");
            var slots = _service.GetAllSlots(venueId,d);
            if(slots == null)
            {
                return BadRequest();
            }
            return Ok(slots);
        }

        // GET api/<SlotController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SlotController>
        [HttpPost]
        public void Post([FromBody] PutSlotDTO s)
        {
           
        }

        // PUT api/<SlotController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PutSlotDTO s)
        {
            var x = _service.UpdateSlot(id, s);
            if (x != null)
            {
                return Ok(x);
            }
            return BadRequest();
        }

        // DELETE api/<SlotController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
