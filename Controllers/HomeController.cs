
using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers 
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ApiKeyAttribute]
        public IActionResult Get()
        {
            return Ok();
        }

        
    }
}