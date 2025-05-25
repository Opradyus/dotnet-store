using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class SliderGetModel
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string Resim { get; set; } = null!;
        public bool Aktif { get; set; }
        public int Index { get; set; }
    
        
    }
