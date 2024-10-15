using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly INCategoryService cService;
        public CategoryController(INCategoryService service) {
            cService = service;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            var c = cService.GetCategoryById(id);
            return Ok(c);
        }
    }
}
