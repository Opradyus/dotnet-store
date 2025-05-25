using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class KategoriGetModel
    {
        
    public int Id {get; set; }

    public string KategoriAdi { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int UrunSayisi { get; set; }
        
    }
