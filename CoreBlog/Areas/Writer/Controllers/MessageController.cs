using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreBlog.Areas.Writer.Controllers
{
    public class MessageController : WriterAreaController
    {
        private readonly IMessageService _messages;
        private readonly IWriterService _writers;

        public MessageController(IMessageService messages, IWriterService writers)
        {
            _messages = messages;
            _writers = writers;
        }

        public IActionResult Index(string box = "inbox")
        {
            var inbox = _messages.GetInbox(CurrentWriter.Id);
            var sent = _messages.GetSent(CurrentWriter.Id);
            return View(new MessageListViewModel
            {
                Box = box == "sent" ? "sent" : "inbox",
                Messages = box == "sent" ? sent : inbox,
                InboxCount = inbox.Count,
                SentCount = sent.Count,
                UnreadCount = inbox.Count(m => !m.IsRead)
            });
        }

        public IActionResult Read(int id)
        {
            var message = _messages.GetForWriter(id, CurrentWriter.Id);
            if (message == null) return NotFound();
            if (message.ReceiverId == CurrentWriter.Id && !message.IsRead)
                _messages.MarkAsRead(id);
            return View(message);
        }

        [HttpGet]
        public IActionResult Compose(int? to, int? replyTo)
        {
            var model = new ComposeMessageViewModel { ReceiverId = to };
            if (replyTo.HasValue)
            {
                var original = _messages.GetForWriter(replyTo.Value, CurrentWriter.Id);
                if (original != null)
                {
                    model.ReceiverId = original.SenderId;
                    model.Subject = original.Subject.StartsWith("Re:") ? original.Subject : "Re: " + original.Subject;
                    model.Details = $"\n\n––– Am {original.Date:dd.MM.yyyy} schrieb {original.Sender?.Name}: –––\n{original.Details}";
                }
            }
            return View(Fill(model));
        }

        [HttpPost]
        public IActionResult Compose(ComposeMessageViewModel model)
        {
            var message = new Message
            {
                SenderId = CurrentWriter.Id,
                ReceiverId = model.ReceiverId,
                Subject = model.Subject?.Trim(),
                Details = model.Details?.Trim(),
                Date = DateTime.Now,
                IsRead = false
            };

            var result = new MessageValidator().Validate(message);
            foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            var receiver = model.ReceiverId.HasValue ? _writers.GetById(model.ReceiverId.Value) : null;
            if (receiver == null || receiver.Id == CurrentWriter.Id)
                ModelState.AddModelError(nameof(model.ReceiverId), "Bitte wählen Sie einen Empfänger.");

            if (!ModelState.IsValid) return View(Fill(model));

            _messages.Add(message);
            Success($"Ihre Nachricht an {receiver.Name} wurde gesendet.");
            return RedirectToAction(nameof(Index), new { box = "sent" });
        }

        [HttpPost]
        public IActionResult Delete(int id, string box = "inbox")
        {
            var message = _messages.GetForWriter(id, CurrentWriter.Id);
            if (message == null) return NotFound();
            _messages.Delete(message);
            Success("Die Nachricht wurde gelöscht.");
            return RedirectToAction(nameof(Index), new { box });
        }

        private ComposeMessageViewModel Fill(ComposeMessageViewModel model)
        {
            model.Receivers = _writers.GetList()
                .Where(w => w.Id != CurrentWriter.Id && w.Status)
                .Select(w => new SelectListItem(w.Name, w.Id.ToString(), w.Id == model.ReceiverId))
                .ToList();
            return model;
        }
    }
}
