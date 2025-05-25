using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class UrunController : Controller
{
    // Dependecy Injection => DI
    private readonly DataContext _context;
    public UrunController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index(int? kategori)
    {
        var query = _context.Urunler.AsQueryable();
        if (kategori != null)
        {
            query = query.Where(i => i.KategoriId == kategori);

        }

        var urun = query.Select(p => new UrunGetModel
        {
            Aciklama = p.Aciklama,
            Aktif = p.Aktif,
            Anasayfa = p.Anasayfa,
            Fiyat = p.Fiyat,
            Id = p.Id,
            Resim = p.Resim,
            UrunAdi = p.UrunAdi,
            UrunKategori = p.Kategori.KategoriAdi
        }).ToList();

        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi", kategori);

        return View(urun);
    }

    [AllowAnonymous]
    public ActionResult List(string url, string q)
    {
        var query = _context.Urunler.Where(i => i.Aktif); //Queryable

        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(p => p.Kategori.Url == url);
        }
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(p => p.UrunAdi.ToLower().Contains(q.ToLower()));
        }
        ViewData["q"] = q;
        return View(query.ToList());
    }
    // [HttpGet("urun/list")]
    // public ActionResult List()
    // {
    //     var urunler = _context.Urunler.Where(p => p.Aktif).ToList();
    //     return View(urunler);
    // }
    [AllowAnonymous]
    public ActionResult Details(int id)
    {
        var urun = _context.Urunler
        .Where(p => p.Id == id)
        .Include(p => p.Kategori)
        .FirstOrDefault();
        if (urun == null)
        {
            return RedirectToAction("Index", "Home");
        }


        ViewData["BenzerUrunler"] = _context.Urunler.
        Where(p => p.Aktif && p.KategoriId == urun.KategoriId && p.Id != id)
        .Take(4)
        .ToList();

        return View(urun);
    }

    [HttpGet]

    public ActionResult Create()
    {
        // ViewData["Kategoriler"]=_context.Kategoriler.ToList();
        // ViewBag.Kategoriler=_context.Kategoriler.ToList();
        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(UrunCreateModel model)
    {
        if (model.Resim == null || model.Resim.Length == 0)
        {
            ModelState.AddModelError("Resim", "Resim Seçmelisiniz");
        }

        if (ModelState.IsValid)
        {
            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Resim!.CopyToAsync(stream);
            }
            var entity = new Urun
            {
                UrunAdi = model.UrunAdi,
                Aciklama = model.Aciklama,
                Fiyat = model.Fiyat ?? 0,
                Aktif = model.Aktif,
                Anasayfa = model.Anasayfa,
                KategoriId = (int)model.KategoriId!,
                Resim = fileName
            };
            _context.Urunler.Add(entity);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
        return View(model);
    }

    public ActionResult Edit(int id)
    {
        var entity = _context.Urunler.Select(i => new UrunEditModel
        {
            Id = i.Id,
            Aciklama = i.Aciklama,
            Aktif = i.Aktif,
            Anasayfa = i.Anasayfa,
            Fiyat = i.Fiyat,
            KategoriId = i.KategoriId,
            ResimAdi = i.Resim,
            UrunAdi = i.UrunAdi
        }).FirstOrDefault(i => i.Id == id);
        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");


        return View(entity);
    }

    [HttpPost]
    public async Task<ActionResult> EditAsync(int id, UrunEditModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            var entity = _context.Urunler.FirstOrDefault(i => i.Id == id);
            if (entity != null)
            {
                if (model.Resim != null)
                {
                    var fileName = Path.GetRandomFileName() + ".jpg";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.Resim!.CopyToAsync(stream);
                    }
                    entity.Resim = fileName;
                }

                entity.Aktif = model.Aktif;
                entity.Aciklama = model.Aciklama;
                entity.Anasayfa = model.Anasayfa;
                entity.Fiyat = model.Fiyat ?? 0;
                entity.KategoriId = (int)model.KategoriId!;
                entity.UrunAdi = model.UrunAdi;
                _context.SaveChanges();
                TempData["Mesaj"] = $"{entity.UrunAdi} Ürünü güncellendi.";
                return RedirectToAction("Index");
            }

        }
        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");

        return View(model);


    }
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }
        var entity = _context.Urunler.FirstOrDefault(i => i.Id == id);
        if (entity != null)
        {
            return View(entity);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteConfirm(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }
        var entity = _context.Urunler.FirstOrDefault(i => i.Id == id);

        if (entity != null)
        {
            _context.Urunler.Remove(entity);
            _context.SaveChanges();
            TempData["Mesaj"] = $"{entity.UrunAdi} Ürünü Silindi.";
        }
        return RedirectToAction("Index");
    }
}