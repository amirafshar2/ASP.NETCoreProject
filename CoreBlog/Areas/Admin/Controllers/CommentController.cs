using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    public class CommentController : AdminAreaController
    {
        private readonly ICommentService _comments;
        public CommentController(ICommentService comments) => _comments = comments;

        public IActionResult Index(string status = "all")
        {
            var list = _comments.GetListWithBlog();
            ViewBag.AllCount = list.Count;
            ViewBag.PendingCount = list.Count(c => !c.Status);
            if (status == "pending") list = list.Where(c => !c.Status).ToList();
            if (status == "approved") list = list.Where(c => c.Status).ToList();
            ViewBag.Status = status;
            return View(list);
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id, string status = "all")
        {
            var comment = _comments.GetById(id);
            if (comment == null) return NotFound();
            _comments.ToggleStatus(id);
            Success(comment.Status ? "Der Kommentar ist jetzt öffentlich sichtbar." : "Der Kommentar wurde ausgeblendet.");
            return RedirectToAction(nameof(Index), new { status });
        }

        [HttpPost]
        public IActionResult Delete(int id, string status = "all")
        {
            var comment = _comments.GetById(id);
            if (comment == null) return NotFound();
            _comments.Delete(comment);
            Success("Der Kommentar wurde gelöscht.");
            return RedirectToAction(nameof(Index), new { status });
        }
    }
}
