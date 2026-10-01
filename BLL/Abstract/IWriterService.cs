using BE.Concrete;

namespace BLL.Abstract
{
    public interface IWriterService : IGenericService<Writer>
    {
        Writer GetByMail(string mail);
        Writer GetByAppUserId(int appUserId);
        List<Writer> GetListWithBlogs();
        void ToggleStatus(int id);
    }
}
