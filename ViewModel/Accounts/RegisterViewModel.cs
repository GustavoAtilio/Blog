

using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel.Accaounts;


public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigadorio")]
    public string Name {get; set;}

    [Required(ErrorMessage = "O E-mail é inválido")]
    [EmailAddress(ErrorMessage = "o E-mail é enválido")]
    public string Email{get; set;}


    [Required(ErrorMessage = "O Senha é inválida")]
    public string Password {get; set;}


}