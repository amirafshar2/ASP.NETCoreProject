using BE.Concrete;
using BLL.Abstract;
using DAL.Abstract;

namespace BLL.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDAL _notificationDal;
        public NotificationManager(INotificationDAL notificationDal) => _notificationDal = notificationDal;

        public void Add(Notification t) => _notificationDal.Insert(t);
        public void Update(Notification t) => _notificationDal.Update(t);
        public void Delete(Notification t) => _notificationDal.Delete(t);
        public Notification GetById(int id) => _notificationDal.GetById(id);
        public List<Notification> GetList() => _notificationDal.GetListAll().OrderByDescending(n => n.Date).ToList();
        public int Count() => _notificationDal.Count();
        public List<Notification> GetActiveList() => _notificationDal.GetListAll(n => n.Status).OrderByDescending(n => n.Date).ToList();

        public void ToggleStatus(int id)
        {
            var n = _notificationDal.GetById(id);
            if (n == null) return;
            n.Status = !n.Status;
            _notificationDal.Update(n);
        }
    }
}
