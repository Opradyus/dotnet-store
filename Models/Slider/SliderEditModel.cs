using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class SliderEditModel
    {
        public int Id { get; set; }

        [Display(Name = "Slider Başlığı") ]
        [Required(ErrorMessage = "Slider Başlığı boş olamaz")]
        [StringLength(40, ErrorMessage = "Slider Başlığı 40 karakterden fazla olamaz")]
        public string? Baslik { get; set; }
        
        [Display(Name = "Slider Açıklaması") ]
        [Required(ErrorMessage = "Slider Açıklaması boş olamaz")]
        [StringLength(200, ErrorMessage = "Slider Açıklaması 200 karakterden fazla olamaz")]
        public string? Aciklama { get; set; }

        [Display(Name = "Mevcut Slider Resmi") ]
        public string ?ResimYolu { get; set; }

        [Display(Name = "Değiştirmek İstediğiniz Slider Resmi") ]
        public IFormFile? Resim { get; set; } 

        [Display(Name = "Slider Aktif mi") ]
        public bool Aktif { get; set; }

        public int Index { get; set; }

    }
