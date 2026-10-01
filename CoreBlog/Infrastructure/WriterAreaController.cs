using BE.Concrete;
using BLL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CoreBlog.Infrastructure
{
    /// <summary>Basisklasse für den Autorenbereich: stellt das Autorenprofil des angemeldeten Benutzers bereit.</summary>
    [Area("Writer")]
    [Authorize(Roles = DataSeeder.WriterRole)]
    public abstract class WriterAreaController : Controller
    {
        private Writer _currentWriter;

        protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        protected Writer CurrentWriter => _currentWriter ??=
            HttpContext.RequestServices.GetRequiredService<IWriterService>().GetByAppUserId(CurrentUserId);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (CurrentWriter == null || !CurrentWriter.Status)
            {
                TempData["error"] = "Ihr Autorenkonto ist gesperrt oder nicht vorhanden.";
                context.Result = RedirectToAction("AccessDenied", "Account", new { area = "" });
                return;
            }
            ViewBag.CurrentWriter = CurrentWriter;
            base.OnActionExecuting(context);
        }

        protected void Success(string message) => TempData["success"] = message;
        protected void Error(string message) => TempData["error"] = message;
    }
}
