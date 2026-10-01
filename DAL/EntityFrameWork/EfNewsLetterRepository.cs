using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfNewsLetterRepository : GenericRepository<NewsLetter>, INewsLetterDAL
    {
        public EfNewsLetterRepository(Context context) : base(context) { }
    }
}
