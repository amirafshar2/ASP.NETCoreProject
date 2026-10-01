using BE.Concrete;

namespace DAL.Abstract
{
    public interface IBlogRatingDAL : IGenericDAL<BlogRating>
    {
        BlogRating GetByBlogId(int blogId);
    }
}
