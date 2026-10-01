using BE.Concrete;

namespace BLL.Abstract
{
    public interface ICommentService : IGenericService<Comment>
    {
        List<Comment> GetListWithBlog();
        List<Comment> GetListByWriter(int writerId);
        void ToggleStatus(int id);
    }
}
