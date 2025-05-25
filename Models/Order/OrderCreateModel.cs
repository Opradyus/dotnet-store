using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models
{
    public class OrderCreateModel
    {
 
        public string AdSoyad { get; set; } = null!;

        public string Sehir { get; set; } = null!;

        public string AdresSatiri { get; set; } = null!;

        public string PostaKodu { get; set; } = null!;

        public string Telefon { get; set; } = null!;

        public string? SiparisNotu { get; set; }

    }
}