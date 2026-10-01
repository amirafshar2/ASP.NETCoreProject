using BE.Concrete;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreBlog.Controllers
{
    /// <summary>
    /// Öffentlicher Demo-Zugang: Besucher (z. B. vom Portfolio) melden sich mit einem Klick
    /// ohne Registrierung als Admin oder als Autorin an.
    /// </summary>
    public class DemoController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly DemoOptions _demo;

        public DemoController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IOptions<DemoOptions> demo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _demo = demo.Value;
        }

        public IActionResult Index()
        {
            if (!_demo.Enabled) return NotFound();
            return View(_demo);
        }

        /// <summary>/Demo/Admin → direkt ins Admin-Panel.</summary>
        public Task<IActionResult> Admin() =>
            SignInAs(_demo.AdminEmail, Url.Action("Index", "Dashboard", new { area = "Admin" }));

        /// <summary>/Demo/Writer → direkt in den Autorenbereich.</summary>
        public Task<IActionResult> Writer() =>
            SignInAs(_demo.WriterEmail, Url.Action("Index", "Dashboard", new { area = "Writer" }));

        private async Task<IActionResult> SignInAs(string email, string target)
        {
            if (!_demo.Enabled) return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return RedirectToAction("Login", "Account");

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["success"] = $"Willkommen in der Demo! Sie sind als {user.FullName} angemeldet.";
            return LocalRedirect(target);
        }
    }
}
