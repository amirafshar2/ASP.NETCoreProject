using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfWriterRepository : GenericRepository<Writer>, IWriterDAL
    {
        public EfWriterRepository(Context context) : base(context) { }

        public Writer GetByMail(string mail) => _context.Writers.FirstOrDefault(w => w.Mail == mail);

        public Writer GetByAppUserId(int appUserId) => _context.Writers.FirstOrDefault(w => w.AppUserId == appUserId);

        public List<Writer> GetListWithBlogs() => _context.Writers
            .AsNoTracking()
            .Include(w => w.Blogs)
            .Include(w => w.AppUser)
            .OrderBy(w => w.Name)
            .ToList();
    }
}
