using BE.Concrete;

namespace BLL.Abstract
{
    public interface IDashboardService
    {
        Dtos.AdminDashboardDto GetAdminDashboard();
        Dtos.WriterDashboardDto GetWriterDashboard(int writerId);
    }
}
