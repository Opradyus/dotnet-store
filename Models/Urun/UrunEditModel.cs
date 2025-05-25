using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;

    public class UrunEditModel:UrunModel
    {
    public int Id { get; set; }
    public string? ResimAdi { get; set; }
    
    }
