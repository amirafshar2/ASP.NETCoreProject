using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfBlogRepository : GenericRepository<Blog>, IBlogDAL
    {
        public EfBlogRepository(Context context) : base(context) { }

        private IQueryable<Blog> WithDetails() => _context.Blogs
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Writer)
            .Include(b => b.Rating);

        public List<Blog> GetListWithDetails(bool onlyPublished) => WithDetails()
            .Where(b => !onlyPublished || (b.Status && b.Category.Status && b.Writer.Status))
            .OrderByDescending(b => b.CreateDate)
            .ToList();

        public Blog GetByIdWithDetails(int id) => _context.Blogs
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Writer)
            .Include(b => b.Rating)
            .Include(b => b.Comments.Where(c => c.Status).OrderByDescending(c => c.Date))
            .FirstOrDefault(b => b.Id == id);

        public List<Blog> GetListByWriter(int writerId) => WithDetails()
            .Where(b => b.WriterId == writerId)
            .OrderByDescending(b => b.CreateDate)
            .ToList();

        public List<Blog> Search(string term, int? categoryId)
        {
            var query = WithDetails().Where(b => b.Status && b.Category.Status && b.Writer.Status);
            if (categoryId.HasValue)
                query = query.Where(b => b.CategoryId == categoryId.Value);
            if (!string.IsNullOrWhiteSpace(term))
            {
                var t = term.Trim().ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(t)
                                      || b.Summary.ToLower().Contains(t)
                                      || b.Content.ToLower().Contains(t));
            }
            return query.OrderByDescending(b => b.CreateDate).ToList();
        }

        public void IncreaseViewCount(int id)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null) return;
            blog.ViewCount++;
            _context.SaveChanges();
        }
    }
}
