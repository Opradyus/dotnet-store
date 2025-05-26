using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;

namespace dotnet_store.Controllers;

    [Authorize(Roles = "Admin")]
    public class SliderController:Controller
    {
        private readonly DataContext _context;
        private readonly ImageProcessingService _imageProcessingService;

    public SliderController(DataContext context, ImageProcessingService imageProcessingService)
    {
        _context = context;
        _imageProcessingService = imageProcessingService;
    }

        public ActionResult Index()
        {
            var slider = _context.Sliderlar.Select(p=>new SliderGetModel{
                Id = p.Id,
                  Aktif = p.Aktif,
                   Baslik=p.Baslik,
                    Index=p.Index,
                     Resim=p.Resim
            }).ToList();
            return View(slider);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public async Task<ActionResult> Create(SliderCreateModel model)
        {
            if (model.Resim == null || model.Resim.Length == 0)
                return View(model);

            if (ModelState.IsValid)
            {
                var fileName = Path.GetRandomFileName() + ".jpeg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using var inputStream = model.Resim.OpenReadStream();
                await _imageProcessingService.ResizeAndSaveAsync(inputStream, path);


                var entity = new Slider
                {
                    Baslik = model.Baslik,
                    Aciklama = model.Aciklama,
                    Aktif = model.Aktif,
                    Resim = fileName,
                    Index = model.Index
                };

                _context.Sliderlar.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);

        }
        public ActionResult Edit(int id)
        {
            var urun = _context.Sliderlar.Select(i=> new SliderEditModel{
                     Aciklama=i.Aciklama,
                      Aktif=i.Aktif,
                       Baslik=i.Baslik,
                        ResimYolu=i.Resim,
                         Id=i.Id, 
                          Index=i.Index                 
            }).FirstOrDefault(i=> i.Id == id);
            return View(urun);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id,SliderEditModel model)
        {
            if(id != model.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
            var entity = _context.Sliderlar.FirstOrDefault(i=> i.Id == id);
            if(entity != null){
                if (model.Resim != null)
                {
                    var fileName = Path.GetRandomFileName() + ".jpeg";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using var inputStream = model.Resim.OpenReadStream();
                    await _imageProcessingService.ResizeAndSaveAsync(inputStream, path);

                    entity.Resim = fileName;
                }
                entity.Aciklama=model.Aciklama;
                entity.Baslik=model.Baslik;
                entity.Aktif=model.Aktif;
                entity.Index=model.Index;
                _context.SaveChanges();
                TempData["Mesaj"]=$"{entity.Baslik} Slider'i güncellendi.";
                return RedirectToAction("Index");
            }
            }
            return View(model);
        }
        public ActionResult Delete(int? id)
    {
        if(id == null)
        {
            return RedirectToAction("Index");
        }
        var entity = _context.Sliderlar.FirstOrDefault(i=> i.Id == id);
        if(entity != null)
        {
            return View(entity);
        }
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public ActionResult DeleteConfirm(int? id)
    {
         if(id == null)
            {
                return RedirectToAction("Index");
            }
            var entity = _context.Sliderlar.FirstOrDefault(i=> i.Id == id);

            if(entity != null)
            {
                _context.Sliderlar.Remove(entity);
                _context.SaveChanges();
                TempData["Mesaj"]=$"{entity.Baslik} Slider'i Silindi.";
            }
            return RedirectToAction("Index");
    }
    }
