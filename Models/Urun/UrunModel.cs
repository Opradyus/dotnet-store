using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;
public class UrunModel
 {
    [Display(Name = "Ürün İsmi")]
    [Required(ErrorMessage ="{0} zorunludur.")]
    [StringLength(50, MinimumLength =5 ,ErrorMessage = "{0} için maksimum {1} karakter, minimum {2} karakter  girmelisiniz.")]
    public string UrunAdi { get; set; } = null!;



    [Display(Name = "Ürün Fiyatı")]
    [Required(ErrorMessage = "{0} zorunludur." )]
    [Range(0,100000,ErrorMessage = "{0} için girdiğiniz fiyat {1} ile {2} arasında olmalıdır.")]
    public double? Fiyat { get; set; }




    [Display(Name = "Ürün Resmi")]
    public IFormFile? Resim { get; set; }




    [Display(Name = "Ürün Açıklaması")]
    public string? Aciklama { get; set; }



    [Display(Name = "Anasayfa Ürünü'mü?")]
    public bool Anasayfa { get; set; }
    
    
    
    [Display(Name = "Ürün Stokta mı ?")]
    public bool Aktif { get; set; }


    
    
    [Display(Name = "Ürün Kategorisi")]
    [Required(ErrorMessage = "{0} zorunludur." )]
    public int? KategoriId { get; set; }
 }