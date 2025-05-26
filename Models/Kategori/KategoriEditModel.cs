using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class KategoriEditModel
    {

    public int Id { get; set; }   

    [Display(Name ="Kategori Adı")]
    [Required]
    [StringLength(50)]
    public string KategoriAdi { get; set; } = null!;

    [Display(Name ="Adres")]
    [Required]
    [StringLength(30)]
    public string Url { get; set; } = null!;
        
    }
