using BE.Concrete;
using BLL.Abstract;
using BLL.ValidationRules;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    public class CategoryController : AdminAreaController
    {
        private readonly ICategoryService _categories;
        private readonly IBlogService _blogs;

        public CategoryController(ICategoryService categories, IBlogService blogs)
        {
            _categories = categories;
            _blogs = blogs;
        }

        public IActionResult Index() => View(_categories.GetListWithBlogCount());

        [HttpGet]
        public IActionResult Create() => View("Edit", new Category { Status = true, Color = "#2F5BEA" });

        [HttpPost]
        public IActionResult Create(Category model)
        {
            var result = new CategoryValidator().Validate(model);
            if (!result.IsValid)
            {
                AddErrors(result);
                return View("Edit", model);
            }
            model.Id = 0;
            _categories.Add(model);
            Success($"Die Kategorie „{model.Name}“ wurde angelegt.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categories.GetById(id);
            return category == null ? NotFound() : View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            var category = _categories.GetById(model.Id);
            if (category == null) return NotFound();

            var result = new CategoryValidator().Validate(model);
            if (!result.IsValid)
            {
                AddErrors(result);
                return View(model);
            }
            category.Name = model.Name.Trim();
            category.Description = model.Description.Trim();
            category.Color = model.Color;
            category.Status = model.Status;
            _categories.Update(category);
            Success("Die Kategorie wurde gespeichert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            _categories.ToggleStatus(id);
            Success("Der Status der Kategorie wurde geändert.");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _categories.GetById(id);
            if (category == null) return NotFound();
            if (_blogs.GetList().Any(b => b.CategoryId == id))
            {
                Error("Kategorien mit Beiträgen können nicht gelöscht werden. Deaktivieren Sie die Kategorie stattdessen.");
                return RedirectToAction(nameof(Index));
            }
            _categories.Delete(category);
            Success($"Die Kategorie „{category.Name}“ wurde gelöscht.");
            return RedirectToAction(nameof(Index));
        }
    }
}
