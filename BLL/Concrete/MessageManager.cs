using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDAL _messageDal;
        public MessageManager(IMessageDAL messageDal) => _messageDal = messageDal;

        public void Add(Message t) => _messageDal.Insert(t);
        public void Update(Message t) => _messageDal.Update(t);
        public void Delete(Message t) => _messageDal.Delete(t);
        public Message GetById(int id) => _messageDal.GetById(id);
        public List<Message> GetList() => _messageDal.GetListAll();
        public int Count() => _messageDal.Count();
        public List<Message> GetInbox(int writerId) => _messageDal.GetInbox(writerId);
        public List<Message> GetSent(int writerId) => _messageDal.GetSent(writerId);
        public int UnreadCount(int writerId) => _messageDal.Count(m => m.ReceiverId == writerId && !m.IsRead);

        /// <summary>Liefert eine Nachricht nur, wenn der Autor Sender oder Empfänger ist.</summary>
        public Message GetForWriter(int id, int writerId)
        {
            var m = _messageDal.GetByIdWithWriters(id);
            return m != null && (m.SenderId == writerId || m.ReceiverId == writerId) ? m : null;
        }

        public void MarkAsRead(int id)
        {
            var m = _messageDal.GetById(id);
            if (m == null || m.IsRead) return;
            m.IsRead = true;
            _messageDal.Update(m);
        }
    }
}
