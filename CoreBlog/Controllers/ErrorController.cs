using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{code:int}")]
        public IActionResult Index(int code)
        {
            Response.StatusCode = code;
            ViewBag.Code = code;
            return View("Error");
        }
    }
}
