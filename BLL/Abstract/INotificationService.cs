using BE.Concrete;

namespace BLL.Abstract
{
    public interface INotificationService : IGenericService<Notification>
    {
        List<Notification> GetActiveList();
        void ToggleStatus(int id);
    }
}
