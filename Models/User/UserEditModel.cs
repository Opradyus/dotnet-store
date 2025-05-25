using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class UserEditModel
{

    public int Id { get; set; }

    [Display(Name = "Ad-Soyad")]
    [Required(ErrorMessage = "Kullanıcı Adı Zorunludur.")]
    // [RegularExpression("[a-zA-Z0-9]*$",ErrorMessage ="Sadece sayı ve harf giriniz.")]
    public string AdSoyad { get; set; } = null!;


    [Display(Name = "E-Posta")]
    [Required(ErrorMessage = "E-Posta Zorunludur.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Display(Name = "Parola")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }


    [Display(Name = "Parola Doğrula")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Parolalar Eşleşmelidir.")]
    public string? ConfirmPassword { get; set; }

    [Display(Name = "Rol")]
    public IList<string>? SelectedRole { get; set; }
    public List<AppRole>? AllRoles { get; set; }


}