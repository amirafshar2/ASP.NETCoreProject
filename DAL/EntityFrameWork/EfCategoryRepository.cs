using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfCategoryRepository : GenericRepository<Category>, ICategoryDAL
    {
        public EfCategoryRepository(Context context) : base(context) { }

        public List<Category> GetListWithBlogCount() => _context.Categories
            .AsNoTracking()
            .Include(c => c.Blogs.Where(b => b.Status))
            .OrderBy(c => c.Name)
            .ToList();
    }
}
