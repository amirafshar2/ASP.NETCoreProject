using BE.Concrete;

namespace BLL.Abstract
{
    public interface IMessageService : IGenericService<Message>
    {
        List<Message> GetInbox(int writerId);
        List<Message> GetSent(int writerId);
        Message GetForWriter(int id, int writerId);
        int UnreadCount(int writerId);
        void MarkAsRead(int id);
    }
}
