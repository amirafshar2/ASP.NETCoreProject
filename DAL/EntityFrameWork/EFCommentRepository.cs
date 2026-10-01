using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfCommentRepository : GenericRepository<Comment>, ICommentDAL
    {
        public EfCommentRepository(Context context) : base(context) { }

        public List<Comment> GetListWithBlog() => _context.Comments
            .AsNoTracking()
            .Include(c => c.Blog)
            .OrderByDescending(c => c.Date)
            .ToList();

        public List<Comment> GetListByWriter(int writerId) => _context.Comments
            .AsNoTracking()
            .Include(c => c.Blog)
            .Where(c => c.Blog.WriterId == writerId)
            .OrderByDescending(c => c.Date)
            .ToList();
    }
}
