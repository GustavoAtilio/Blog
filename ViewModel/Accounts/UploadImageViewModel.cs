

using System.ComponentModel.DataAnnotations;

namespace Blog.Services.Accaounts;


public class UploadImageViewModel
{
    [Required(ErrorMessage = "Imagem inválida")]
    public string Base64Image {get; set;}
}