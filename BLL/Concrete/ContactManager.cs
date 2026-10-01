using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IContactDAL _contactDal;
        public ContactManager(IContactDAL contactDal) => _contactDal = contactDal;

        public void Add(Contact t) => _contactDal.Insert(t);
        public void Update(Contact t) => _contactDal.Update(t);
        public void Delete(Contact t) => _contactDal.Delete(t);
        public Contact GetById(int id) => _contactDal.GetById(id);
        public List<Contact> GetList() => _contactDal.GetListAll().OrderByDescending(c => c.Date).ToList();
        public int Count() => _contactDal.Count();
        public int UnreadCount() => _contactDal.Count(c => !c.IsRead);

        public void MarkAsRead(int id)
        {
            var c = _contactDal.GetById(id);
            if (c == null || c.IsRead) return;
            c.IsRead = true;
            _contactDal.Update(c);
        }
    }
}
