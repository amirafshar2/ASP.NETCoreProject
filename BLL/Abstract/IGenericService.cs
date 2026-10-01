using BE.Concrete;

namespace BLL.Abstract
{
    public interface IGenericService<T>
    {
        void Add(T t);
        void Update(T t);
        void Delete(T t);
        T GetById(int id);
        List<T> GetList();
        int Count();
    }
}
