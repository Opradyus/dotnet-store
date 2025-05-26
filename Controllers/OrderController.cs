using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;


[Authorize]
public class OrderController : Controller
{



    private readonly ICartService _cartService;
    private readonly DataContext _context;
    public OrderController(ICartService cartService, DataContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Index()
    {
        return View(_context.Orders.ToList());
    }
    [Authorize(Roles = "Admin")]
    public ActionResult Details(int id)
    {
        return View();
    }
    public async Task<ActionResult> CheckOut()
    {
        ViewBag.Cart = await _cartService.GetCart(User.Identity?.Name!);
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CheckOut(OrderCreateModel model)
    {
        var username = User.Identity?.Name!;
        var cart = await _cartService.GetCart(username);

        if (cart.CartItems.Count == 0)
        {
            ModelState.AddModelError("", "Sepetinizde ürün yok");
        }

        if (ModelState.IsValid)
        {
            var order = new Order
            {
                AdSoyad = model.AdSoyad,
                AdresSatiri = model.AdresSatiri,
                Telefon = model.Telefon,
                PostaKodu = model.PostaKodu,
                Sehir = model.Sehir,
                SiparisNotu = model.SiparisNotu!,
                SiparisTarihi = DateTime.Now,
                ToplamFiyat = cart.Toplam(),
                Username = username,
                OrderItems = cart.CartItems.Select(i => new OrderItem
                {
                    UrunId = i.UrunId,
                    Fiyat = i.Urun.Fiyat,
                    Miktar = i.Miktar
                }).ToList()
            };


            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Completed", new { orderId = order.Id });
        }
        ;
        ViewBag.Cart = cart;
        return View(model);

    }

    public ActionResult Completed(string orderId)
    {
        return View("Completed",orderId);
    }




}