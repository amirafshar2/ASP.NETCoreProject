using DAL.Abstract;
using DAL.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    /// <summary>Generisches Repository – der Context wird per Dependency Injection (Scoped) übergeben.</summary>
    public class GenericRepository<T> : IGenericDAL<T> where T : class
    {
        protected readonly Context _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public void Insert(T t)
        {
            _context.Add(t);
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            _context.Update(t);
            _context.SaveChanges();
        }

        public void Delete(T t)
        {
            _context.Remove(t);
            _context.SaveChanges();
        }

        public T GetById(int id) => _context.Set<T>().Find(id);

        public List<T> GetListAll() => _context.Set<T>().AsNoTracking().ToList();

        public List<T> GetListAll(Expression<Func<T, bool>> filter) =>
            _context.Set<T>().AsNoTracking().Where(filter).ToList();

        public int Count(Expression<Func<T, bool>> filter = null) =>
            filter == null ? _context.Set<T>().Count() : _context.Set<T>().Count(filter);
    }
}
