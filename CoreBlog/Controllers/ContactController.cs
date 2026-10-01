using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contacts;
        public ContactController(IContactService contacts) => _contacts = contacts;

        [HttpGet]
        public IActionResult Index() => View(new Contact());

        [HttpPost]
        public IActionResult Index(Contact contact)
        {
            var result = new ContactValidator().Validate(contact);
            if (!result.IsValid)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(contact);
            }

            contact.Id = 0;
            contact.Date = DateTime.Now;
            contact.IsRead = false;
            _contacts.Add(contact);
            TempData["success"] = "Danke für Ihre Nachricht! Wir melden uns in Kürze.";
            return RedirectToAction(nameof(Index));
        }
    }
}
