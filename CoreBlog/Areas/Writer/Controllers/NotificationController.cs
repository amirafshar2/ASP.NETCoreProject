using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Writer.Controllers
{
    public class NotificationController : WriterAreaController
    {
        private readonly INotificationService _notifications;
        public NotificationController(INotificationService notifications) => _notifications = notifications;

        public IActionResult Index() => View(_notifications.GetActiveList());
    }
}
