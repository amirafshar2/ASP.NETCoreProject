using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class WriterManager : IWriterService
    {
        private readonly IWriterDAL _writerDal;
        public WriterManager(IWriterDAL writerDal) => _writerDal = writerDal;

        public void Add(Writer t) => _writerDal.Insert(t);
        public void Update(Writer t) => _writerDal.Update(t);
        public void Delete(Writer t) => _writerDal.Delete(t);
        public Writer GetById(int id) => _writerDal.GetById(id);
        public List<Writer> GetList() => _writerDal.GetListAll().OrderBy(w => w.Name).ToList();
        public int Count() => _writerDal.Count();
        public Writer GetByMail(string mail) => _writerDal.GetByMail(mail);
        public Writer GetByAppUserId(int appUserId) => _writerDal.GetByAppUserId(appUserId);
        public List<Writer> GetListWithBlogs() => _writerDal.GetListWithBlogs();

        public void ToggleStatus(int id)
        {
            var w = _writerDal.GetById(id);
            if (w == null) return;
            w.Status = !w.Status;
            _writerDal.Update(w);
        }
    }
}
