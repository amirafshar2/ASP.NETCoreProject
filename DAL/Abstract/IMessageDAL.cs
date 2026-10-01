using BE.Concrete;

namespace DAL.Abstract
{
    public interface IMessageDAL : IGenericDAL<Message>
    {
        List<Message> GetInbox(int writerId);
        List<Message> GetSent(int writerId);
        Message GetByIdWithWriters(int id);
    }
}
