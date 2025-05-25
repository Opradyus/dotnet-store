using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.ViewComponents
{
    public class Topbar:ViewComponent
    {
        private UserManager<AppUser> _userManager;

        public Topbar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
         public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = user != null
                ? await _userManager.GetRolesAsync(user)
                : new List<string>();

            return View(roles); // Views/Shared/Components/Topbar/Default.cshtml
        }

    }
}