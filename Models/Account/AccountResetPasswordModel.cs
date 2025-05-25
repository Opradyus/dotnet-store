using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountResetPasswordModel
{

    public string Token { get; set; }= null!;
    public string Email { get; set; }= null!;

    [Display(Name = "Parola")]
    [Required(ErrorMessage = "Parola Zorunludur")]
    [DataType(DataType.Password)] 
    public string Password { get; set; } = null!;

    
    [Display(Name = "Yeni Parola'yı Doğrula")]
    [Required(ErrorMessage ="Lütfen Parolayı doğrulayın")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "Parolalar Eşleşmelidir.")]
    public string ConfirmPassword { get; set; } = null!;


}