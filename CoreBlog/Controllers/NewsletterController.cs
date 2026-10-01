using BLL.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoreBlog.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly INewsLetterService _newsLetters;
        public NewsletterController(INewsLetterService newsLetters) => _newsLetters = newsLetters;

        [HttpPost]
        public IActionResult Subscribe(string mail)
        {
            if (string.IsNullOrWhiteSpace(mail) || !new EmailAddressAttribute().IsValid(mail.Trim()))
                return Json(new { ok = false, message = "Bitte geben Sie eine gültige E-Mail-Adresse ein." });

            return _newsLetters.Subscribe(mail)
                ? Json(new { ok = true, message = "Danke! Sie erhalten ab jetzt unseren Newsletter." })
                : Json(new { ok = true, message = "Diese Adresse ist bereits eingetragen." });
        }
    }
}
