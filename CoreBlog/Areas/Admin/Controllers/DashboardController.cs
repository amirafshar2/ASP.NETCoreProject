using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Admin.Controllers
{
    public class DashboardController : AdminAreaController
    {
        private readonly IDashboardService _dashboard;
        public DashboardController(IDashboardService dashboard) => _dashboard = dashboard;

        public IActionResult Index() => View(_dashboard.GetAdminDashboard());
    }
}
