using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfMessageRepository : GenericRepository<Message>, IMessageDAL
    {
        public EfMessageRepository(Context context) : base(context) { }

        public List<Message> GetInbox(int writerId) => _context.Messages
            .AsNoTracking()
            .Include(m => m.Sender)
            .Where(m => m.ReceiverId == writerId)
            .OrderByDescending(m => m.Date)
            .ToList();

        public List<Message> GetSent(int writerId) => _context.Messages
            .AsNoTracking()
            .Include(m => m.Receiver)
            .Where(m => m.SenderId == writerId)
            .OrderByDescending(m => m.Date)
            .ToList();

        public Message GetByIdWithWriters(int id) => _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .FirstOrDefault(m => m.Id == id);
    }
}
