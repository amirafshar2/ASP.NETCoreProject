using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _comments;
        private readonly IBlogService _blogs;

        public CommentController(ICommentService comments, IBlogService blogs)
        {
            _comments = comments;
            _blogs = blogs;
        }

        [HttpPost]
        public IActionResult Add(CommentForm form)
        {
            var blog = _blogs.GetById(form.BlogId);
            if (blog == null || !blog.Status) return NotFound();

            var comment = new Comment
            {
                BlogId = form.BlogId,
                UserName = form.UserName?.Trim(),
                Email = form.Email?.Trim(),
                Title = form.Title?.Trim(),
                Content = form.Content?.Trim(),
                Score = form.Score,
                Date = DateTime.Now,
                Status = true
            };

            var result = new CommentValidator().Validate(comment);
            if (!result.IsValid)
            {
                TempData["CommentErrors"] = string.Join("|", result.Errors.Select(e => e.ErrorMessage));
                TempData["CommentDraft"] = System.Text.Json.JsonSerializer.Serialize(form);
                return Redirect(Url.Action("Detail", "Blog", new { id = form.BlogId }) + "#kommentieren");
            }

            _comments.Add(comment);
            TempData["success"] = "Danke! Ihr Kommentar wurde veröffentlicht.";
            return Redirect(Url.Action("Detail", "Blog", new { id = form.BlogId }) + "#kommentare");
        }
    }
}
