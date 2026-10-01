using BE.Concrete;

namespace DAL.Abstract
{
    public interface IBlogDAL : IGenericDAL<Blog>
    {
        /// <summary>Blogs mit Kategorie, Autor und Bewertung (optional nur veröffentlichte).</summary>
        List<Blog> GetListWithDetails(bool onlyPublished);
        Blog GetByIdWithDetails(int id);
        List<Blog> GetListByWriter(int writerId);
        List<Blog> Search(string term, int? categoryId);
        void IncreaseViewCount(int id);
    }
}
