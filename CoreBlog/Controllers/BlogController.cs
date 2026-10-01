using BLL.Abstract;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogs;
        private readonly IWriterService _writers;

        public BlogController(IBlogService blogs, IWriterService writers)
        {
            _blogs = blogs;
            _writers = writers;
        }

        /// <summary>Alle Beiträge – leitet auf die Startseite mit Filter weiter.</summary>
        public IActionResult Index(int? category, string q, int page = 1) =>
            RedirectToAction("Index", "Home", new { category, q, page });

        public IActionResult Detail(int id)
        {
            var blog = _blogs.GetDetail(id);
            if (blog == null || !blog.Status || !blog.Category.Status || !blog.Writer.Status)
            {
                // Autoren und Admins dürfen ihre Entwürfe als Vorschau sehen
                var canPreview = blog != null && User.Identity?.IsAuthenticated == true &&
                                 (User.IsInRole(DataSeeder.AdminRole) || User.Identity.Name == blog.Writer.Mail);
                if (!canPreview) return NotFound();
                ViewBag.IsPreview = true;
            }
            else
            {
                // Aufrufe nur einmal pro Sitzung zählen (Cookie)
                var cookie = $"cb_v_{id}";
                if (!Request.Cookies.ContainsKey(cookie))
                {
                    _blogs.RegisterView(id);
                    Response.Cookies.Append(cookie, "1", new CookieOptions { HttpOnly = true, IsEssential = true, SameSite = SameSiteMode.Lax });
                }
            }

            var model = new BlogDetailViewModel
            {
                Blog = blog,
                Related = _blogs.GetRelated(blog, 3),
                Headings = ContentFormatter.Headings(blog.Content),
                Comment = new CommentForm { BlogId = blog.Id },
                WriterBlogCount = _blogs.GetListByWriter(blog.WriterId).Count(b => b.Status)
            };
            if (TempData["CommentErrors"] is string errors)
                ViewBag.CommentErrors = errors.Split('|');
            return View(model);
        }

        public IActionResult Author(int id)
        {
            var writer = _writers.GetById(id);
            if (writer == null || !writer.Status) return NotFound();

            var blogs = _blogs.GetListByWriter(id).Where(b => b.Status && b.Category.Status).ToList();
            var rated = blogs.Where(b => b.Rating?.RatingCount > 0).ToList();
            return View(new AuthorViewModel
            {
                Writer = writer,
                Blogs = blogs,
                TotalViews = blogs.Sum(b => b.ViewCount),
                AverageRating = rated.Count == 0 ? 0
                    : Math.Round((double)rated.Sum(b => b.Rating.TotalScore) / rated.Sum(b => b.Rating.RatingCount), 1)
            });
        }
    }
}
