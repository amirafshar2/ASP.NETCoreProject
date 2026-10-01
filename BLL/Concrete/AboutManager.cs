using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class AboutManager : IAboutService
    {
        private readonly IAboutDAL _aboutDal;
        public AboutManager(IAboutDAL aboutDal) => _aboutDal = aboutDal;

        public void Add(About t) => _aboutDal.Insert(t);
        public void Update(About t) => _aboutDal.Update(t);
        public void Delete(About t) => _aboutDal.Delete(t);
        public About GetById(int id) => _aboutDal.GetById(id);
        public List<About> GetList() => _aboutDal.GetListAll();
        public int Count() => _aboutDal.Count();
        public About GetCurrent() => _aboutDal.GetListAll().FirstOrDefault();
    }
}
