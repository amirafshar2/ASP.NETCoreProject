using BLL.Abstract;
using CoreBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _about;
        private readonly IWriterService _writers;
        private readonly IBlogService _blogs;
        private readonly ICommentService _comments;
        private readonly ICategoryService _categories;

        public AboutController(IAboutService about, IWriterService writers, IBlogService blogs,
            ICommentService comments, ICategoryService categories)
        {
            _about = about;
            _writers = writers;
            _blogs = blogs;
            _comments = comments;
            _categories = categories;
        }

        public IActionResult Index()
        {
            return View(new AboutViewModel
            {
                About = _about.GetCurrent(),
                Writers = _writers.GetListWithBlogs().Where(w => w.Status).ToList(),
                BlogCount = _blogs.GetPublishedList().Count,
                CommentCount = _comments.GetList().Count(c => c.Status),
                CategoryCount = _categories.GetActiveList().Count
            });
        }
    }
}
