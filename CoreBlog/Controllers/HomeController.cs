using BLL.Abstract;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 6;
        private readonly IBlogService _blogs;
        private readonly ICategoryService _categories;
        private readonly IWriterService _writers;

        public HomeController(IBlogService blogs, ICategoryService categories, IWriterService writers)
        {
            _blogs = blogs;
            _categories = categories;
            _writers = writers;
        }

        public IActionResult Index(int page = 1, int? category = null, string q = null)
        {
            var model = new HomeViewModel
            {
                Categories = _categories.GetListWithBlogCount().Where(c => c.Status).ToList(),
                Popular = _blogs.GetPopular(4),
                Writers = _writers.GetListWithBlogs().Where(w => w.Status && w.Blogs.Any(b => b.Status)).ToList(),
                Search = q?.Trim()
            };
            if (category.HasValue)
                model.ActiveCategory = model.Categories.FirstOrDefault(c => c.Id == category.Value);

            if (model.IsFiltered)
            {
                model.Blogs = new PagedList<BE.Concrete.Blog>(_blogs.Search(model.Search, model.ActiveCategory?.Id), page, PageSize);
            }
            else
            {
                var all = _blogs.GetPublishedList();
                model.Featured = all.FirstOrDefault(b => b.IsFeatured) ?? all.FirstOrDefault();
                var rest = all.Where(b => b.Id != model.Featured?.Id).ToList();
                model.Secondary = rest.Take(3).ToList();
                model.Blogs = new PagedList<BE.Concrete.Blog>(rest.Skip(3), page, PageSize);
            }
            return View(model);
        }
    }
}
