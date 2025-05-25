using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountPasswordEditModel
{
    [Display(Name = "Eski Parola")]
    [Required(ErrorMessage = "Parola Zorunludur")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Yeni Parola")]
    [Required(ErrorMessage = "Parola Zorunludur")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;
    
    [Display(Name = "Yeni Parola'yı Doğrula")]
    [Required(ErrorMessage ="Lütfen Parolayı doğrulayın")]
    [DataType(DataType.Password)]
    [Compare("NewPassword",ErrorMessage = "Parolalar Eşleşmelidir.")]
    public string ConfirmNewPassword { get; set; } = null!;


}