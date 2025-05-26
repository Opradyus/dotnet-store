using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;
    public class KategoriCreateModel
    {
    
    [Required]
    [StringLength(50)]
    [Display(Name ="Kategori Adı")]
    public string KategoriAdi { get; set; } = null!;

    [Display(Name ="URL")]  
    [Required]
    [StringLength(30)]
    public string Url { get; set; } = null!;

        
    }
