using BE.Concrete;
using DAL.Abstract;
using DAL.Concrete;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework
{
    public class EfNotificationRepository : GenericRepository<Notification>, INotificationDAL
    {
        public EfNotificationRepository(Context context) : base(context) { }
    }
}
