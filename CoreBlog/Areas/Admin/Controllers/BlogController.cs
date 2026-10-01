using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CoreBlog.Areas.Admin.Controllers
{
    public class BlogController : AdminAreaController
    {
        private readonly IBlogService _blogs;
        private readonly ICategoryService _categories;
        private readonly IWriterService _writers;

        public BlogController(IBlogService blogs, ICategoryService categories, IWriterService writers)
        {
            _blogs = blogs;
            _categories = categories;
            _writers = writers;
        }

        public IActionResult Index(string q, int? category, int? writer, string status = "all")
        {
            var list = _blogs.GetListWithDetails();
            if (!string.IsNullOrWhiteSpace(q))
                list = list.Where(b => b.Title.Contains(q.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (category.HasValue) list = list.Where(b => b.CategoryId == category).ToList();
            if (writer.HasValue) list = list.Where(b => b.WriterId == writer).ToList();
            if (status == "published") list = list.Where(b => b.Status).ToList();
            if (status == "draft") list = list.Where(b => !b.Status).ToList();

            ViewBag.Categories = _categories.GetList();
            ViewBag.Writers = _writers.GetList();
            ViewBag.Query = q;
            ViewBag.Category = category;
            ViewBag.Writer = writer;
            ViewBag.Status = status;
            return View(list);
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var blog = _blogs.GetById(id);
            if (blog == null) return NotFound();
            _blogs.ToggleStatus(id);
            Success(blog.Status ? $"„{blog.Title}“ ist jetzt veröffentlicht." : $"„{blog.Title}“ wurde zurückgezogen.");
            return Redirect(Request.Headers.Referer.ToString() is { Length: > 0 } r ? r : Url.Action(nameof(Index)));
        }

        [HttpPost]
        public IActionResult Feature(int id)
        {
            var blog = _blogs.GetById(id);
            if (blog == null) return NotFound();
            _blogs.SetFeatured(id);
            Success($"„{blog.Title}“ wird jetzt als Titelgeschichte auf der Startseite gezeigt.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var blog = _blogs.GetById(id);
            if (blog == null) return NotFound();
            _blogs.Delete(blog);
            Success($"„{blog.Title}“ wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Export aller Beiträge als CSV (öffnet sich direkt in Excel).</summary>
        public IActionResult Export()
        {
            var csv = new StringBuilder();
            csv.AppendLine("ID;Titel;Kategorie;Autor;Datum;Status;Aufrufe;Bewertung;Kommentare");
            foreach (var b in _blogs.GetListWithDetails())
            {
                csv.AppendLine(string.Join(";",
                    b.Id, Escape(b.Title), Escape(b.Category?.Name), Escape(b.Writer?.Name),
                    b.CreateDate.ToString("dd.MM.yyyy"), b.Status ? "veröffentlicht" : "Entwurf",
                    b.ViewCount, (b.Rating?.Average ?? 0).ToString("0.0"), b.Rating?.RatingCount ?? 0));
            }
            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
            return File(bytes, "text/csv", $"blogs-{DateTime.Now:yyyy-MM-dd}.csv");
        }

        private static string Escape(string value) => "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";
    }
}
