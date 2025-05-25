using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class UrunGetModel
    {
    public int Id { get; set; }
    public string UrunAdi { get; set; } = null!;
    public double Fiyat { get; set; }
    public string? Resim { get; set; }
    public string? Aciklama { get; set; }
    public bool Anasayfa { get; set; }
    public bool Aktif { get; set; }
    public string UrunKategori { get; set; } = null!;
        
    }
