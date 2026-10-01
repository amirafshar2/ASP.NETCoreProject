using BE.Concrete;

namespace DAL.Abstract
{
    public interface ICategoryDAL : IGenericDAL<Category>
    {
        List<Category> GetListWithBlogCount();
    }
}
