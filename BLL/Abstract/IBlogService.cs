using BE.Concrete;

namespace BLL.Abstract
{
    public interface IBlogService : IGenericService<Blog>
    {
        List<Blog> GetPublishedList();
        List<Blog> GetListWithDetails();
        Blog GetDetail(int id);
        List<Blog> GetListByWriter(int writerId);
        List<Blog> GetLatest(int count);
        List<Blog> GetPopular(int count);
        Blog GetFeatured();
        List<Blog> GetRelated(Blog blog, int count);
        List<Blog> Search(string term, int? categoryId);
        void RegisterView(int id);
        void ToggleStatus(int id);
        void SetFeatured(int id);
    }
}
