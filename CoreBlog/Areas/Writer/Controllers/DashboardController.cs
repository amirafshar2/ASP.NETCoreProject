using BLL.Abstract;
using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CoreBlog.Areas.Writer.Controllers
{
    public class DashboardController : WriterAreaController
    {
        private readonly IDashboardService _dashboard;
        public DashboardController(IDashboardService dashboard) => _dashboard = dashboard;

        public IActionResult Index() => View(_dashboard.GetWriterDashboard(CurrentWriter.Id));
    }
}
