using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    /// <summary>Bearbeitung der Seite „Über uns“.</summary>
    public class AboutController : AdminAreaController
    {
        private readonly IAboutService _about;
        private readonly ImageUploader _uploader;

        public AboutController(IAboutService about, ImageUploader uploader)
        {
            _about = about;
            _uploader = uploader;
        }

        [HttpGet]
        public IActionResult Index() => View(new AboutEditViewModel { About = _about.GetCurrent() ?? new About() });

        [HttpPost]
        public IActionResult Index(AboutEditViewModel model)
        {
            var current = _about.GetCurrent();
            var tracked = current == null ? new About() : _about.GetById(current.Id);

            var result = new AboutValidator().Validate(model.About);
            foreach (var e in result.Errors) ModelState.AddModelError("About." + e.PropertyName, e.ErrorMessage);

            var (img1, err1) = _uploader.Save(model.Image1File, "about");
            var (img2, err2) = _uploader.Save(model.Image2File, "about");
            if (err1 != null) ModelState.AddModelError(nameof(model.Image1File), err1);
            if (err2 != null) ModelState.AddModelError(nameof(model.Image2File), err2);

            if (!result.IsValid || err1 != null || err2 != null)
            {
                model.About.Image1 = tracked.Image1;
                model.About.Image2 = tracked.Image2;
                return View(model);
            }

            tracked.Title = model.About.Title;
            tracked.Details1 = model.About.Details1;
            tracked.Details2 = model.About.Details2;
            tracked.MapLocation = model.About.MapLocation;
            tracked.Status = true;
            if (img1 != null) tracked.Image1 = img1;
            if (img2 != null) tracked.Image2 = img2;

            if (current == null) _about.Add(tracked); else _about.Update(tracked);
            Success("Die Seite „Über uns“ wurde gespeichert.");
            return RedirectToAction(nameof(Index));
        }
    }
}
