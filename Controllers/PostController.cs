
using Blog.Data;
using Blog.Models;
using Blog.ViewModel.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetPost
    (
        [FromServices]BlogDataContext context,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25
    )
    {
        var count = await context.Posts.AsNoTracking().CountAsync();
        var posts = await context.Posts
                                 .AsNoTracking()
                                 .Include(x => x.Author)
                                 .Include(x => x.Category)
                                 .Skip(page * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        if(posts == null)
            return BadRequest(new ResultViewModel<string>("Não Encontrado Registros"));

        return Ok(new ResultViewModel<dynamic>(new 
        {
            total = count,
            page,
            pageSize,
            posts
        }));
    }
}