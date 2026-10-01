using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfBlogRatingRepository : GenericRepository<BlogRating>, IBlogRatingDAL
    {
        public EfBlogRatingRepository(Context context) : base(context) { }

        public BlogRating GetByBlogId(int blogId) => _context.BlogRatings.FirstOrDefault(r => r.BlogId == blogId);
    }
}
