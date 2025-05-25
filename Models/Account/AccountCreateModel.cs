using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountCreateModel
{

    [Display(Name = "Ad-Soyad")]
    [Required(ErrorMessage ="Kullanıcı Adı Zorunludur.")]
    // [RegularExpression("[a-zA-Z0-9]*$",ErrorMessage ="Sadece sayı ve harf giriniz.")]
    public string AdSoyad { get; set; } = null!;


    [Display(Name = "Parola")]
    [Required(ErrorMessage ="Parola Zorunludur")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Display(Name = "Parola")]
    [Required(ErrorMessage ="Lütfen Parolayı doğrulayın")]
    [DataType(DataType.Password)]
    [Compare("Password",ErrorMessage = "Parolalar Eşleşmelidir.")]
    public string ConfirmPassword { get; set; } = null!;

     
    [Display(Name = "E-Posta")]
    [Required(ErrorMessage ="E-Posta Zorunludur.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

}