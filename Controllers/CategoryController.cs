

using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories/{id:int}")]
        public IActionResult Get(
            [FromServices]BlogDataContext context,
            [FromRoute] int id
            )
        {
            Thread.Sleep(id);
            //var categories = context.Categories.ToList();
            return Ok();
        }
    }
}