using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace dotnet_store.Models;
public class Kategori
{
    public int Id {get; set; }

    public string KategoriAdi { get; set; } = null!;

    public string Url { get; set; } = null!;

    public List<Urun> Uruns { get; set; } = new();


}
