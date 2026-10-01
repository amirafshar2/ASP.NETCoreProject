using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class NewsLetterManager : INewsLetterService
    {
        private readonly INewsLetterDAL _newsLetterDal;
        public NewsLetterManager(INewsLetterDAL newsLetterDal) => _newsLetterDal = newsLetterDal;

        public void Add(NewsLetter t) => _newsLetterDal.Insert(t);
        public void Update(NewsLetter t) => _newsLetterDal.Update(t);
        public void Delete(NewsLetter t) => _newsLetterDal.Delete(t);
        public NewsLetter GetById(int id) => _newsLetterDal.GetById(id);
        public List<NewsLetter> GetList() => _newsLetterDal.GetListAll().OrderByDescending(n => n.Date).ToList();
        public int Count() => _newsLetterDal.Count();

        public bool Subscribe(string mail)
        {
            var normalized = mail.Trim().ToLowerInvariant();
            if (_newsLetterDal.Count(n => n.Mail == normalized) > 0) return false;
            _newsLetterDal.Insert(new NewsLetter { Mail = normalized, Status = true, Date = DateTime.Now });
            return true;
        }
    }
}
