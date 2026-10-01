using BE.Concrete;
using BLL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CoreBlog.Infrastructure
{
    /// <summary>Basisklasse für das Admin-Panel.</summary>
    [Area("Admin")]
    [Authorize(Roles = DataSeeder.AdminRole)]
    public abstract class AdminAreaController : Controller
    {
        protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        protected void Success(string message) => TempData["success"] = message;
        protected void Error(string message) => TempData["error"] = message;

        protected void AddErrors(FluentValidation.Results.ValidationResult result)
        {
            foreach (var e in result.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
        }
    }
}
