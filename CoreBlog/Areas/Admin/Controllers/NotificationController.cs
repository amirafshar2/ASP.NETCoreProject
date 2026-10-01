using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    /// <summary>Hinweise, die im Autorenbereich angezeigt werden.</summary>
    public class NotificationController : AdminAreaController
    {
        public static readonly (string Icon, string Label)[] Icons =
        {
            ("fa-solid fa-bullhorn", "Ankündigung"), ("fa-solid fa-calendar-check", "Termin"),
            ("fa-solid fa-circle-info", "Information"), ("fa-solid fa-triangle-exclamation", "Warnung"),
            ("fa-solid fa-screwdriver-wrench", "Wartung"), ("fa-solid fa-image", "Bilder"),
            ("fa-solid fa-hand-sparkles", "Willkommen"), ("fa-solid fa-laptop-house", "Remote")
        };

        private readonly INotificationService _notifications;
        public NotificationController(INotificationService notifications) => _notifications = notifications;

        public IActionResult Index() => View(_notifications.GetList());

        [HttpGet]
        public IActionResult Create() => View("Edit", new Notification { TypeSymbol = Icons[0].Icon, SymbolColor = "#2F5BEA", Status = true });

        [HttpPost]
        public IActionResult Create(Notification model)
        {
            var result = new NotificationValidator().Validate(model);
            if (!result.IsValid)
            {
                AddErrors(result);
                return View("Edit", model);
            }
            model.Id = 0;
            model.Date = DateTime.Now;
            model.TypeSymbol = Icons.Any(i => i.Icon == model.TypeSymbol) ? model.TypeSymbol : Icons[0].Icon;
            _notifications.Add(model);
            Success("Der Hinweis wurde veröffentlicht.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var n = _notifications.GetById(id);
            return n == null ? NotFound() : View(n);
        }

        [HttpPost]
        public IActionResult Edit(Notification model)
        {
            var n = _notifications.GetById(model.Id);
            if (n == null) return NotFound();
            var result = new NotificationValidator().Validate(model);
            if (!result.IsValid)
            {
                AddErrors(result);
                return View(model);
            }
            n.Type = model.Type;
            n.Details = model.Details;
            n.SymbolColor = model.SymbolColor;
            n.TypeSymbol = Icons.Any(i => i.Icon == model.TypeSymbol) ? model.TypeSymbol : n.TypeSymbol;
            n.Status = model.Status;
            _notifications.Update(n);
            Success("Der Hinweis wurde gespeichert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            _notifications.ToggleStatus(id);
            Success("Der Status des Hinweises wurde geändert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var n = _notifications.GetById(id);
            if (n == null) return NotFound();
            _notifications.Delete(n);
            Success("Der Hinweis wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }
    }
}
