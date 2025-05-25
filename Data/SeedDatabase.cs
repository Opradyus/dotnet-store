using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Models;

public static class SeedDatabase
{
    public static async void Initialize(IApplicationBuilder app)
    {
        var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        if (!roleManager.Roles.Any())
        {
            var admin = new AppRole { Name = "Admin" };
            await roleManager.CreateAsync(admin);

        }
        if (!userManager.Users.Any())
        {
            var admin = new AppUser
            {
                AdSoyad = "Admin",
                UserName = "Admin123",
                Email = "Admin@outlook.com",
            };
            await userManager.CreateAsync(admin, "Admin123");
            await userManager.AddToRoleAsync(admin, "Admin");
            var customer = new AppUser
            {
                AdSoyad = "Customer",
                UserName = "Customer123",
                Email = "Customer@outlook.com",
            };
        
            await userManager.CreateAsync(customer, "Customer123");
        }
    }
}