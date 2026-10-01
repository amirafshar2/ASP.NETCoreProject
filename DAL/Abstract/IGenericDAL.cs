using System.Linq.Expressions;

namespace DAL.Abstract
{
    public interface IGenericDAL<T> where T : class
    {
        void Insert(T t);
        void Update(T t);
        void Delete(T t);
        T GetById(int id);
        List<T> GetListAll();
        List<T> GetListAll(Expression<Func<T, bool>> filter);
        int Count(Expression<Func<T, bool>> filter = null);
    }
}
