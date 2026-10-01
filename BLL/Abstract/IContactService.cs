using BE.Concrete;

namespace BLL.Abstract
{
    public interface IContactService : IGenericService<Contact>
    {
        void MarkAsRead(int id);
        int UnreadCount();
    }
}
