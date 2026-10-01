using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDAL _categoryDal;
        public CategoryManager(ICategoryDAL categoryDal) => _categoryDal = categoryDal;

        public void Add(Category t) => _categoryDal.Insert(t);
        public void Update(Category t) => _categoryDal.Update(t);
        public void Delete(Category t) => _categoryDal.Delete(t);
        public Category GetById(int id) => _categoryDal.GetById(id);
        public List<Category> GetList() => _categoryDal.GetListAll().OrderBy(c => c.Name).ToList();
        public int Count() => _categoryDal.Count();
        public List<Category> GetListWithBlogCount() => _categoryDal.GetListWithBlogCount();
        public List<Category> GetActiveList() => _categoryDal.GetListAll(c => c.Status).OrderBy(c => c.Name).ToList();

        public void ToggleStatus(int id)
        {
            var c = _categoryDal.GetById(id);
            if (c == null) return;
            c.Status = !c.Status;
            _categoryDal.Update(c);
        }
    }
}
