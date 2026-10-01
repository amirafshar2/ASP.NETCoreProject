using BE.Concrete;

namespace DAL.Abstract
{
    public interface IWriterDAL : IGenericDAL<Writer>
    {
        Writer GetByMail(string mail);
        Writer GetByAppUserId(int appUserId);
        List<Writer> GetListWithBlogs();
    }
}
