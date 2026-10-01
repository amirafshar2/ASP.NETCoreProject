using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CoreBlog.Areas.Admin.Controllers
{
    public class NewsletterController : AdminAreaController
    {
        private readonly INewsLetterService _newsLetters;
        public NewsletterController(INewsLetterService newsLetters) => _newsLetters = newsLetters;

        public IActionResult Index() => View(_newsLetters.GetList());

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entry = _newsLetters.GetById(id);
            if (entry == null) return NotFound();
            _newsLetters.Delete(entry);
            Success($"{entry.Mail} wurde aus dem Verteiler entfernt.");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Export()
        {
            var csv = new StringBuilder("E-Mail;Angemeldet am\n");
            foreach (var n in _newsLetters.GetList())
                csv.AppendLine($"{n.Mail};{n.Date:dd.MM.yyyy}");
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
            return File(bytes, "text/csv", $"newsletter-{DateTime.Now:yyyy-MM-dd}.csv");
        }
    }
}
