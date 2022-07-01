

using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public  async Task<IActionResult> GetAsync(
            [FromServices]BlogDataContext context,
            [FromRoute] int id
            )
        {
            var categories = await context.Categories
                                          .AsNoTracking()
                                          .Include(x => x.Posts)
                                          .ToListAsync();
            
            return Ok(new ResultViewModel<List<Category>>(categories));
        }

         [HttpGet("v1/categories/{id:int}")]
        public  async Task<IActionResult> GetByIdAsync(
            [FromServices]BlogDataContext context,
            [FromRoute] int id
            )
        {
           
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null) return NotFound(new ResultViewModel<Category>("Conteúdo Não Encontrado"));
            return Ok(new ResultViewModel<Category>(category));
        }

        [HttpPost("v1/categories")]
        public  async Task<IActionResult> PostAsync(
            [FromServices]BlogDataContext context,
            [FromBody]CategoriesViewModel category
            )
        {
            if(!ModelState.IsValid){
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }
            await context.Categories.AddAsync(new Category{
                Name = category.Name,
                Slug = category.Slug
            });
            await context.SaveChangesAsync();
            return Created($"v1/categories/", new ResultViewModel<CategoriesViewModel>(category));
        }
    }
}