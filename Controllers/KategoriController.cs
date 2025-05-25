using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace dotnet_store.Controllers;

    [Authorize(Roles ="Admin")]
    public class KategoriController:Controller
    {
        private readonly DataContext _context;

        public KategoriController(DataContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var kategoriler = _context.Kategoriler.Select(i=> new KategoriGetModel
            {
            Id = i.Id,
            KategoriAdi = i.KategoriAdi,
            Url =i.Url,
            UrunSayisi = i.Uruns.Count
            }).ToList();

            return View(kategoriler);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(KategoriCreateModel model)
        {
            
            if(ModelState.IsValid){

            var entity = new Kategori
            {
                 KategoriAdi=model.KategoriAdi,
                 Url=model.Url,
            };
            _context.Kategoriler.Add(entity);
            _context.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
        
            var entity = _context.Kategoriler.Select(i=> new KategoriEditModel
            {
                 Id=i.Id,
                 KategoriAdi=i.KategoriAdi,
                 Url=i.Url
            }).FirstOrDefault(i=> i.Id == id);

            return View(entity);
            
        }
        [HttpPost]
        public ActionResult Edit(int id, KategoriEditModel model)
            {
                if(id != model.Id)
                {
                    return NotFound();
                }
                if(ModelState.IsValid){
                    var entity = _context.Kategoriler.FirstOrDefault(i=> i.Id==model.Id);
                    if(entity != null)
                    {
                        entity.KategoriAdi = model.KategoriAdi;
                        entity.Url = model.Url;
                        _context.SaveChanges();    
                        TempData["Mesaj"]=$"{entity.KategoriAdi} kategorisi güncellendi.";
                        return RedirectToAction("Index");        
                    }
                }
                

                return View(model);
            }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var entity = _context.Kategoriler
                                .Include(k => k.Uruns)
                                .FirstOrDefault(i => i.Id == id);

            if (entity == null)
            {
                return RedirectToAction("Index");
            }

            if (entity.Uruns != null && entity.Uruns.Any())
            {
                TempData["Mesaj"] = $"{entity.KategoriAdi} kategorisine bağlı ürünler olduğu için silinemez.(:D GOtcha)";
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int? id)
        {
             if(id == null)
            {
                return RedirectToAction("Index");
            }
            var entity = _context.Kategoriler.FirstOrDefault(i=> i.Id == id);

            if(entity != null)
            {
                _context.Kategoriler.Remove(entity);
                _context.SaveChanges();
                TempData["Mesaj"]=$"{entity.KategoriAdi} kategorisi Silindi.";
            }
            return RedirectToAction("Index");
        }
    }
