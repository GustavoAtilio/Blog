using System.Text.RegularExpressions;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.Services.Accaounts;
using Blog.ViewModel;
using Blog.ViewModel.Accaounts;
using Blog.ViewModel.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;


[ApiController]
public class AccountController : ControllerBase 
{
     [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post (
        [FromBody]  RegisterViewModel model,
        [FromServices] BlogDataContext blogDataContext
    )
    {
      if(!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        //var passsword = PasswordGenerator.Generate(25, includeSpecialChars: true, upperCase: false);
        user.PasswordHash = PasswordHasher.Hash(model.Password);

        try
        {
            await blogDataContext.Users.AddAsync(user);
            await blogDataContext.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new 
            {
                user = user.Email, model.Password
            }
            ));
        }catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Este E-mail já existe"));
        }
    }
    
    //[AllowAnonymous]
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromServices] TokenService tokenService,
        [FromServices] BlogDataContext context,
        [FromBody]  LoginViewModel model
    )
    {
        if(!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        
        var user = await context.Users
                                .AsNoTracking()
                                .Include(x => x.Roles)
                                .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido!"));
        
        if (!PasswordHasher.Verify(user.PasswordHash,model.Password))
             return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido!"));
        try
        {
             var token = tokenService.GenerateToken(user);
             return Ok(new ResultViewModel<string>(token, null));
        }catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
        }
    }

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] BlogDataContext context
        )
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"data:image [a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);
        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

        }catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha Interna no sistema"));
        }

        var user = await context.Users
                                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        if(user == null)
             return BadRequest(new ResultViewModel<string>("Usuário não encontrado"));
        user.Image = $"https://localhost:0000/images/{fileName}";
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha Interna no sistema"));
        }
        
        return Ok(new ResultViewModel<User>(user));
    }
  
}