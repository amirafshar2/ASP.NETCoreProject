using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfContactRepository : GenericRepository<Contact>, IContactDAL
    {
        public EfContactRepository(Context context) : base(context) { }
    }
}
