using BE.Concrete;

namespace BLL.Abstract
{
    public interface ICategoryService : IGenericService<Category>
    {
        List<Category> GetListWithBlogCount();
        List<Category> GetActiveList();
        void ToggleStatus(int id);
    }
}
