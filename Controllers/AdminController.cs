using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers; 

    [Authorize(Roles = "Admin")]
    public class AdminController:Controller
    { 
        public IActionResult Index()
        { 
            return View();
        }
        
    }
