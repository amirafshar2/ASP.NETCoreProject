using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Writer.Controllers
{
    /// <summary>Kommentare zu den eigenen Beiträgen.</summary>
    public class CommentController : WriterAreaController
    {
        private readonly ICommentService _comments;
        public CommentController(ICommentService comments) => _comments = comments;

        public IActionResult Index() => View(_comments.GetListByWriter(CurrentWriter.Id));
    }
}
