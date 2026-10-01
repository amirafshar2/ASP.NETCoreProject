using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreBlog.Areas.Writer.Controllers
{
    public class BlogController : WriterAreaController
    {
        private readonly IBlogService _blogs;
        private readonly ICategoryService _categories;
        private readonly ImageUploader _uploader;

        public BlogController(IBlogService blogs, ICategoryService categories, ImageUploader uploader)
        {
            _blogs = blogs;
            _categories = categories;
            _uploader = uploader;
        }

        public IActionResult Index(string status = "all", string q = null)
        {
            var list = _blogs.GetListByWriter(CurrentWriter.Id);
            ViewBag.AllCount = list.Count;
            ViewBag.PublishedCount = list.Count(b => b.Status);
            ViewBag.DraftCount = list.Count(b => !b.Status);
            if (status == "published") list = list.Where(b => b.Status).ToList();
            if (status == "draft") list = list.Where(b => !b.Status).ToList();
            if (!string.IsNullOrWhiteSpace(q))
                list = list.Where(b => b.Title.Contains(q.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.Status = status;
            ViewBag.Query = q;
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View("Edit", Fill(new BlogEditViewModel { Image = "/img/blog/b1.jpg" }));

        [HttpPost]
        public IActionResult Create(BlogEditViewModel model) => Save(model, isNew: true);

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var blog = Own(id);
            if (blog == null) return NotFound();
            return View(Fill(new BlogEditViewModel
            {
                Id = blog.Id, Title = blog.Title, Summary = blog.Summary, Content = blog.Content,
                CategoryId = blog.CategoryId, Status = blog.Status, Image = blog.Image
            }));
        }

        [HttpPost]
        public IActionResult Edit(BlogEditViewModel model) => Save(model, isNew: false);

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var blog = Own(id);
            if (blog == null) return NotFound();
            _blogs.ToggleStatus(id);
            Success(blog.Status ? "Der Beitrag wurde veröffentlicht." : "Der Beitrag ist jetzt ein Entwurf.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var blog = Own(id);
            if (blog == null) return NotFound();
            _blogs.Delete(blog);
            Success($"„{blog.Title}“ wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }

        private IActionResult Save(BlogEditViewModel model, bool isNew)
        {
            Blog blog;
            if (isNew)
            {
                blog = new Blog { WriterId = CurrentWriter.Id, CreateDate = DateTime.Now };
            }
            else
            {
                blog = Own(model.Id);
                if (blog == null) return NotFound();
            }

            blog.Title = model.Title?.Trim();
            blog.Summary = model.Summary?.Trim();
            blog.Content = model.Content?.Trim();
            blog.CategoryId = model.CategoryId;
            blog.Status = model.Status;

            var result = new BlogValidator().Validate(blog);
            foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);

            var (path, uploadError) = _uploader.Save(model.ImageFile, "blogs");
            if (uploadError != null) ModelState.AddModelError(nameof(model.ImageFile), uploadError);

            if (!result.IsValid || uploadError != null)
            {
                model.Image = path ?? model.Image ?? blog.Image;
                return View("Edit", Fill(model));
            }

            // Eigenes Bild hochgeladen oder eines aus der Galerie gewählt
            var chosen = IsAllowedImage(model.Image) ? model.Image : null;
            blog.Image = path ?? chosen ?? blog.Image ?? "/img/blog/b1.jpg";
            if (isNew) _blogs.Add(blog); else _blogs.Update(blog);

            Success(isNew
                ? (blog.Status ? "Ihr Beitrag wurde veröffentlicht." : "Ihr Entwurf wurde gespeichert.")
                : "Die Änderungen wurden gespeichert.");
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Lädt einen Beitrag nur, wenn er dem angemeldeten Autor gehört.</summary>
        private Blog Own(int id)
        {
            var blog = _blogs.GetById(id);
            return blog != null && blog.WriterId == CurrentWriter.Id ? blog : null;
        }

        private static bool IsAllowedImage(string image) =>
            !string.IsNullOrEmpty(image) && (image.StartsWith("/img/blog/") || image.StartsWith("/uploads/blogs/"))
            && !image.Contains("..");

        private BlogEditViewModel Fill(BlogEditViewModel model)
        {
            model.Categories = _categories.GetActiveList()
                .Select(c => new SelectListItem(c.Name, c.Id.ToString(), c.Id == model.CategoryId))
                .ToList();
            return model;
        }
    }
}
