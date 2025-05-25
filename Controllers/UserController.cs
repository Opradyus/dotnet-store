using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{

    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;

    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ActionResult> Index(string role)
    {
        ViewBag.Roller = new SelectList(_roleManager.Roles, "Name", "Name",role);
        if (!string.IsNullOrEmpty(role))
        {
            return View(await _userManager.GetUsersInRoleAsync(role));
        }

        return View(_userManager.Users);
    }

    public ActionResult Create()
    {

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(UserCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdSoyad = model.AdSoyad
            };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                TempData["Mesaj"] = "Başarılı bir şekilde kullanıcı eklendi.";

                return RedirectToAction("Index", "User");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
    public async Task<ActionResult> Delete(string? id)
    {
        if (id == null)
        {
            TempData["Mesaj"] = "Id geçerli değil.";
            return RedirectToAction("Index");
        }
        var entity = await _userManager.FindByIdAsync(id);

        if (entity == null)
        {
            TempData["Mesaj"] = "Böyle bir kullanıcı yok.";
            return RedirectToAction("Index");
        }
        var roles = await _userManager.GetRolesAsync(entity);
        if (roles.Contains("Admin"))
        {
            TempData["Mesaj"]=("Admin rolüne sahip kullanıcı silinemez.");
            return RedirectToAction("Index");

        }
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }
        return View(entity);
    }

    public async Task<ActionResult> DeleteConfirm(string? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }
        var entity = await _userManager.FindByIdAsync(id);
        if (entity != null)
        {
            var result = await _userManager.DeleteAsync(entity);
            if (result.Succeeded)
            {
                TempData["Mesaj"] = $"{entity.UserName} adlı kullanıcı silindi.";
            }
            // foreach (var error in result.Errors)
            // {
            //     ModelState.AddModelError("", error.Description);
            // }
        }

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return RedirectToAction("Index");
        }

        return View(
            new UserEditModel
            {
                AdSoyad = user.AdSoyad,
                Email = user.Email!,
                Id = user.Id,
                SelectedRole = await _userManager.GetRolesAsync(user),
                AllRoles = await _roleManager.Roles.ToListAsync()
            }
        );
    }

    [HttpPost]
public async Task<ActionResult> Edit(string id, UserEditModel model)
{
    model.AllRoles = await _roleManager.Roles.ToListAsync();
    if (!ModelState.IsValid)
        {
            return View(model);
        }

    if (id == null)
    {
        return RedirectToAction("Index");
    }

    var user = await _userManager.FindByIdAsync(id);
    if (user == null)
    {
        return RedirectToAction("Index");
    }

    user.Email = model.Email;
    user.AdSoyad = model.AdSoyad;


    var updateResult = await _userManager.UpdateAsync(user);
    if (!updateResult.Succeeded)
    {
        foreach (var error in updateResult.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        ViewBag.Roles = await _roleManager.Roles.ToListAsync();
        return View(model);
    }

    if (!string.IsNullOrEmpty(model.Password))
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
        if (!resetResult.Succeeded)
        {
            foreach (var error in resetResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(model);
        }
    }
    

    if (model.SelectedRole != null)
    {
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRolesAsync(user, model.SelectedRole);
    }

    TempData["Mesaj"] = $"{model.AdSoyad} adlı kullanıcı bilgisi başarıyla güncellendi.";
    return RedirectToAction("Index");
}

}
