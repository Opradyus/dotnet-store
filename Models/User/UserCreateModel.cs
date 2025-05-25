using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class UserCreateModel
{

    [Display(Name = "Ad-Soyad")]
    [Required(ErrorMessage ="Kullanıcı Adı Zorunludur.")]
    // [RegularExpression("[a-zA-Z0-9]*$",ErrorMessage ="Sadece sayı ve harf giriniz.")]
    public string AdSoyad { get; set; } = null!;

     
    [Display(Name = "E-Posta")]
    [Required(ErrorMessage ="E-Posta Zorunludur.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

}