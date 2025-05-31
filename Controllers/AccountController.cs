using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{

    private readonly ICartService _cartService;
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _singInManager;

    private readonly DataContext _cartManager;

    private IEmailService _emailService;
    public AccountController(UserManager<AppUser> userManager,
                            SignInManager<AppUser> singInManager,
                            IEmailService emailService,
                            DataContext cartManager,
                            ICartService cartService)
    {
        _userManager = userManager;
        _singInManager = singInManager;
        _emailService = emailService;
        _cartManager = cartManager;
        _cartService = cartService;
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(AccountCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdSoyad = model.AdSoyad
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["Mesaj"] = "Başarılı bir şekilde kayıt oldunuz.";

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

        }

        return View(model);
    }

    public ActionResult Login()
    {

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(AccountLoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _singInManager.SignOutAsync();
                var result = await _singInManager.PasswordSignInAsync(user, model.Password, model.BeniHatirla, true);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    await _cartService.TransferCartToUser(user.UserName!);

                    if (!string.IsNullOrEmpty(model.returnUrl) && Url.IsLocalUrl(model.returnUrl))//Model Binding ASP.NET Core'un model binding motoru onu HTTP GET query string’inden alıp senin metot parametrene bağladı.Vay amk
                    {
                        return Redirect(model.returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (result.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kitlendi. Lütfen {timeLeft.Minutes + 1} dakika bekleyiniz.");
                }
                else
                {
                    ModelState.AddModelError("", "Parola Eşleşmiyor");
                }
            }
            else
            {
                ModelState.AddModelError("", "Hatalı Email");
            }
        }
        return View(model);
    }

    [Authorize]
    public async Task<ActionResult> LogOutAsync()
    {
        await _singInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");

    }

    [Authorize]
    public ActionResult Settings()
    {

        return View();

    }


    public ActionResult AccesDenied()
    {

        return View();
    }

    [Authorize]
    public async Task<ActionResult> EditUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new AccountEditUserModel
        {
            AdSoyad = user.AdSoyad,
            Email = user.Email!

        });
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> EditUser(AccountEditUserModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user != null)
            {
                user.Email = model.Email;
                user.AdSoyad = model.AdSoyad;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Mesaj"] = "Kullanıcı bilgileri değişti";
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(model);
        }
        return View(model);
    }

    [Authorize]
    public ActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangePassword(AccountPasswordEditModel model)
    {
        if (ModelState.IsValid)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

                if (result.Succeeded)
                {
                    TempData["Mesaj"] = "Şifre bilgileriniz başarıyla değişti";
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

        }
        return View(model);
    }

    public ActionResult ForgotPassword()
    {

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["Mesaj"] = "E-Mail Adresinizi giriniz.";
            return View();
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["Mesaj"] = "Lütfen kayıtlı bir E-Mail adresi giriniz.";
            return View();
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });
        var link = $"<a href='http://localhost:5162{url}'>Şifre Yenile</a>";
        await _emailService.SendEmailAsync(user.Email!,"ParolaSıfırlama",link);
        TempData["Mesaj"] = "E-Mail adresinizi kontrol ediniz.";

        return RedirectToAction("Login");
    }

    public async Task<ActionResult> ResetPassword(string userId, string token)
    {
        if (userId == null || token == null)
        {

            return RedirectToAction("Login");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var model = new AccountResetPasswordModel
        {
            Token = token,
            Email = user.Email!
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword(AccountResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData["Mesaj"] = "Şifreniz başarıyla değişti.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

}
