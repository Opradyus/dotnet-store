using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Models;

public class AppUser : IdentityUser<int>
{
    public string AdSoyad { get; set; } = null!;   

   
}
