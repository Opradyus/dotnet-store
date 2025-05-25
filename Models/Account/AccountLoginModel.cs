using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountLoginModel
{

    [Display(Name = "Parola")]
    [Required(ErrorMessage ="Parola Zorunludur")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

     
    [Display(Name = "E-Posta")]
    [Required(ErrorMessage ="E-Posta Zorunludur.")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    

    public bool BeniHatirla { get; set; } = true;

    public string? returnUrl { get; set; }

}