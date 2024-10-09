using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BmvContext _bmvContext;

        public CategoryController(BmvContext bmvContext) {
            _bmvContext = bmvContext;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cService = new CategoryService(_bmvContext);
            var c = cService.GetCategoryById(id);
            return Ok(c);
        }
    }
}
