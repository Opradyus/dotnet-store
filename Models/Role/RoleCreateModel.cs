using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_store.Models;
    public class RoleCreateModel
    {
    
    [Required]
    [StringLength(30)]
    [Display(Name ="Rol Adı")]
    public string RoleAdi { get; set; } = null!;
        
    }
