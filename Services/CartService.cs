using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_store.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Services;

public interface ICartService
{
    string GetCustomerId();
    Task<Cart> GetCart(string customerId);
    Task AddToCart(int urunId, int miktar = 1);
    Task RemoveItem(int urunId, int miktar = 1);
    Task TransferCartToUser(string userName);
}

public class CartService : ICartService
{

    private readonly DataContext _cartManager;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(DataContext cartManager, IHttpContextAccessor httpContextAccessor)
    {
        _cartManager = cartManager;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task AddToCart(int urunId, int miktar = 1)
    {
        var cart = await GetCart(GetCustomerId());

        var urun = await _cartManager.Urunler.FirstOrDefaultAsync(i => i.Id == urunId);
        if (urun != null)
        {
            cart.AddItem(urun, miktar);
            await _cartManager.SaveChangesAsync();
        }

    }

    public async Task<Cart> GetCart(string custId)
    {
        var cart = await _cartManager.Carts.Include(i => i.CartItems).ThenInclude(i => i.Urun).Where(i => i.CustomerId == custId).FirstOrDefaultAsync();
        if (cart == null)
        {
            var customerId = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (string.IsNullOrEmpty(customerId))
            {
                customerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("customerId", customerId, cookieOptions);
            }
            ;

            cart = new Cart { CustomerId = customerId };
            _cartManager.Carts.Add(cart);
            await _cartManager.SaveChangesAsync();
        }
            return cart;   
    }
    public string GetCustomerId()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User.Identity?.Name ?? context?.Request.Cookies["CustomerId"]!;
    }

    public async Task RemoveItem(int urunId, int miktar = 1)
    {
        var cart = await GetCart(GetCustomerId());

        var urun = await _cartManager.Urunler.FirstOrDefaultAsync(i => i.Id == urunId);
        if (urun != null)
        {
            cart.DeleteItem(urunId, miktar);
            await _cartManager.SaveChangesAsync();
        }
    }

    public async Task TransferCartToUser(string userName)
    {
        var userCart = await GetCart(userName);
        var cookieCart = await GetCart(_httpContextAccessor.HttpContext?.Request.Cookies["customerId"]!);

        foreach (var item in cookieCart?.CartItems!)
        {
            var cartItem = userCart?.CartItems.Where(i => i.UrunId == item.UrunId).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Miktar += item.Miktar;
            } 
            else
            {
                userCart?.CartItems.Add(new CartItem { UrunId = item.UrunId, Miktar = item.Miktar });
            }
        }
        _cartManager.Carts.Remove(cookieCart);
        await _cartManager.SaveChangesAsync();
    }
}

