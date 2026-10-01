using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreBlog.Areas.Writer.Controllers
{
    public class ProfileController : WriterAreaController
    {
        private readonly IWriterService _writers;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ImageUploader _uploader;
        private readonly DemoOptions _demo;

        public ProfileController(IWriterService writers, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ImageUploader uploader, IOptions<DemoOptions> demo)
        {
            _writers = writers;
            _userManager = userManager;
            _signInManager = signInManager;
            _uploader = uploader;
            _demo = demo.Value;
        }

        [HttpGet]
        public IActionResult Index() => View(new ProfileViewModel
        {
            Name = CurrentWriter.Name,
            About = CurrentWriter.About,
            Email = CurrentWriter.Mail,
            Image = CurrentWriter.Image,
            IsDemoAccount = _demo.IsDemoAccount(CurrentWriter.Mail)
        });

        [HttpPost]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            var writer = _writers.GetById(CurrentWriter.Id);
            model.Email = writer.Mail;
            model.IsDemoAccount = _demo.IsDemoAccount(writer.Mail);

            var check = new BE.Concrete.Writer { Name = model.Name?.Trim(), About = model.About?.Trim() };
            var result = new WriterValidator().Validate(check);
            foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);

            var (path, uploadError) = _uploader.Save(model.ImageFile, "avatars");
            if (uploadError != null) ModelState.AddModelError(nameof(model.ImageFile), uploadError);

            if (!result.IsValid || uploadError != null)
            {
                model.Image = writer.Image;
                return View(model);
            }

            writer.Name = check.Name;
            writer.About = check.About;
            if (path != null) writer.Image = path;
            _writers.Update(writer);

            var user = await _userManager.FindByIdAsync(CurrentUserId.ToString());
            user.FullName = writer.Name;
            user.ImageUrl = writer.Image;
            await _userManager.UpdateAsync(user);

            Success("Ihr Profil wurde gespeichert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Password()
        {
            ViewBag.IsDemoAccount = _demo.IsDemoAccount(CurrentWriter.Mail);
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Password(ChangePasswordViewModel model)
        {
            ViewBag.IsDemoAccount = _demo.IsDemoAccount(CurrentWriter.Mail);
            if (_demo.IsDemoAccount(CurrentWriter.Mail))
            {
                Error("Im Demo-Konto kann das Passwort nicht geändert werden – so bleibt die Demo für alle Besucher erreichbar.");
                return RedirectToAction(nameof(Password));
            }
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(CurrentUserId.ToString());
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            Success("Ihr Passwort wurde geändert.");
            return RedirectToAction(nameof(Index));
        }
    }
}
