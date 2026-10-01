using BE.Concrete;

namespace DAL.Abstract
{
    public interface ICommentDAL : IGenericDAL<Comment>
    {
        List<Comment> GetListWithBlog();
        List<Comment> GetListByWriter(int writerId);
    }
}
