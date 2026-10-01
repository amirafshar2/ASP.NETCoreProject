using BE.Concrete;
using BLL.Abstract;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWriterService _writers;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IWriterService writers)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _writers = writers;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Zu viele Fehlversuche. Bitte versuchen Sie es in einigen Minuten erneut.");
                return View(model);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "E-Mail-Adresse oder Passwort ist nicht korrekt.");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return LocalRedirect(model.ReturnUrl);
            return await RedirectToPanel(user);
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new AppUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                FullName = model.FullName.Trim(),
                ImageUrl = "/img/avatar-default.svg",
                CreatedAt = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, DataSeeder.WriterRole);
            _writers.Add(new Writer
            {
                Name = user.FullName, Mail = user.Email, Image = user.ImageUrl,
                About = "", Status = true, AppUserId = user.Id, CreatedAt = DateTime.Now
            });

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["success"] = $"Willkommen bei CoreBlog, {user.FullName}! Ihr Autorenkonto ist bereit.";
            return RedirectToAction("Index", "Dashboard", new { area = "Writer" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Sie wurden abgemeldet.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AccessDenied() => View();

        private async Task<IActionResult> RedirectToPanel(AppUser user)
        {
            if (await _userManager.IsInRoleAsync(user, DataSeeder.AdminRole))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            return RedirectToAction("Index", "Dashboard", new { area = "Writer" });
        }
    }
}
