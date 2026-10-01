using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreBlog.Areas.Admin.Controllers
{
    /// <summary>Verwaltung der Autoren, ihrer Konten und Rollen.</summary>
    public class WriterController : AdminAreaController
    {
        private readonly IWriterService _writers;
        private readonly UserManager<AppUser> _userManager;
        private readonly DemoOptions _demo;

        public WriterController(IWriterService writers, UserManager<AppUser> userManager, IOptions<DemoOptions> demo)
        {
            _writers = writers;
            _userManager = userManager;
            _demo = demo.Value;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager.GetUsersInRoleAsync(DataSeeder.AdminRole)).Select(u => u.Id).ToHashSet();
            var model = _writers.GetListWithBlogs().Select(w => new WriterAdminViewModel
            {
                Id = w.Id, Name = w.Name, About = w.About, Email = w.Mail, Image = w.Image,
                Status = w.Status, BlogCount = w.Blogs.Count, CreatedAt = w.CreatedAt,
                IsAdmin = w.AppUserId.HasValue && admins.Contains(w.AppUserId.Value),
                IsDemoAccount = _demo.IsDemoAccount(w.Mail)
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var writer = _writers.GetById(id);
            if (writer == null) return NotFound();
            var user = writer.AppUserId.HasValue ? await _userManager.FindByIdAsync(writer.AppUserId.ToString()) : null;
            return View(new WriterAdminViewModel
            {
                Id = writer.Id, Name = writer.Name, About = writer.About, Email = writer.Mail, Image = writer.Image,
                Status = writer.Status, CreatedAt = writer.CreatedAt,
                IsAdmin = user != null && await _userManager.IsInRoleAsync(user, DataSeeder.AdminRole),
                IsDemoAccount = _demo.IsDemoAccount(writer.Mail)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WriterAdminViewModel model)
        {
            var writer = _writers.GetById(model.Id);
            if (writer == null) return NotFound();
            model.Email = writer.Mail;
            model.Image = writer.Image;
            model.IsDemoAccount = _demo.IsDemoAccount(writer.Mail);

            var result = new WriterValidator().Validate(new BE.Concrete.Writer { Name = model.Name, About = model.About });
            if (!result.IsValid)
            {
                AddErrors(result);
                return View(model);
            }

            writer.Name = model.Name.Trim();
            writer.About = model.About?.Trim();
            // Demo-Konten bleiben immer aktiv, damit die Demo erreichbar bleibt
            writer.Status = model.IsDemoAccount || model.Status;
            _writers.Update(writer);

            var user = writer.AppUserId.HasValue ? await _userManager.FindByIdAsync(writer.AppUserId.ToString()) : null;
            if (user != null)
            {
                user.FullName = writer.Name;
                await _userManager.UpdateAsync(user);

                var isAdmin = await _userManager.IsInRoleAsync(user, DataSeeder.AdminRole);
                if (model.IsDemoAccount && isAdmin != model.IsAdmin)
                    Error("Die Rollen der Demo-Konten können nicht geändert werden.");
                else if (user.Id == CurrentUserId && !model.IsAdmin)
                    Error("Sie können sich die Admin-Rolle nicht selbst entziehen.");
                else if (model.IsAdmin && !isAdmin)
                    await _userManager.AddToRoleAsync(user, DataSeeder.AdminRole);
                else if (!model.IsAdmin && isAdmin)
                    await _userManager.RemoveFromRoleAsync(user, DataSeeder.AdminRole);
            }

            Success($"Das Profil von {writer.Name} wurde gespeichert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var writer = _writers.GetById(id);
            if (writer == null) return NotFound();
            if (_demo.IsDemoAccount(writer.Mail))
            {
                Error("Demo-Konten können nicht gesperrt werden.");
                return RedirectToAction(nameof(Index));
            }
            _writers.ToggleStatus(id);
            Success(writer.Status ? $"{writer.Name} ist wieder aktiv." : $"{writer.Name} wurde gesperrt. Die Beiträge sind öffentlich ausgeblendet.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var writer = _writers.GetById(id);
            if (writer == null) return NotFound();
            if (_demo.IsDemoAccount(writer.Mail) || writer.AppUserId == CurrentUserId)
            {
                Error("Dieses Konto kann nicht gelöscht werden.");
                return RedirectToAction(nameof(Index));
            }

            var user = writer.AppUserId.HasValue ? await _userManager.FindByIdAsync(writer.AppUserId.ToString()) : null;
            _writers.Delete(writer);
            if (user != null) await _userManager.DeleteAsync(user);
            Success($"{writer.Name} und alle zugehörigen Beiträge wurden gelöscht.");
            return RedirectToAction(nameof(Index));
        }
    }
}
