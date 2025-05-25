using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers; 

    [Authorize(Roles = "Admin")]
    public class RoleController:Controller
    { 

        private RoleManager<AppRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RoleController(RoleManager<AppRole> rolemanager,UserManager<AppUser> userManager)
        {
            _userManager=userManager;
            _roleManager = rolemanager;         
        }
        public IActionResult Index()
        { 
            return View(_roleManager.Roles);
        }
        
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateModel model)
        { 
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new AppRole{Name=model.RoleAdi});

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }

            return View(model);
        }
        
        public async Task<ActionResult> Edit(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if(entity != null)
            {
                return View(new RoleEditModel{Id=entity.Id,RoleAdi=entity.Name!});
            }
            TempData["Mesaj"] = "Böyle bir rol yok.";
            return RedirectToAction("Index");

        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(string id,RoleEditModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = await _roleManager.FindByIdAsync(id);
                if(entity != null)
                {
                    entity.Name = model.RoleAdi;
                    var result = await _roleManager.UpdateAsync(entity);
                    if(result.Succeeded)
                    {
                        TempData["Mesaj"] = $"{entity.Name} rolü değiştirildi.";
                        return RedirectToAction("Index");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }

            }
            return View(model);
        }

        public async Task<ActionResult> DeleteAsync(string? id)
        {
             if(id == null)
             {
                return RedirectToAction("Index");
             }
             var entity = await _roleManager.FindByIdAsync(id);
             if(entity != null)
             {
                ViewBag.Users= await _userManager.GetUsersInRoleAsync(entity.Name!);
                return View(entity);
             }
             TempData["Mesaj"]="Böyle bir rol yok";
             return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmAsync(string? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var entity = await _roleManager.FindByIdAsync(id);
            if(entity != null)
            {
                await _roleManager.DeleteAsync(entity);
            }
            
            return RedirectToAction("Index");
        }

    }
