using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    /// <summary>Nachrichten aus dem Kontaktformular.</summary>
    public class ContactController : AdminAreaController
    {
        private readonly IContactService _contacts;
        public ContactController(IContactService contacts) => _contacts = contacts;

        public IActionResult Index() => View(_contacts.GetList());

        public IActionResult Read(int id)
        {
            var contact = _contacts.GetById(id);
            if (contact == null) return NotFound();
            _contacts.MarkAsRead(id);
            return View(contact);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var contact = _contacts.GetById(id);
            if (contact == null) return NotFound();
            _contacts.Delete(contact);
            Success("Die Nachricht wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }
    }
}
