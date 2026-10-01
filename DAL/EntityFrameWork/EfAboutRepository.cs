using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfAboutRepository : GenericRepository<About>, IAboutDAL
    {
        public EfAboutRepository(Context context) : base(context) { }
    }
}
