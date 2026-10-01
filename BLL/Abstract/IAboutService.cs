using BE.Concrete;

namespace BLL.Abstract
{
    public interface IAboutService : IGenericService<About>
    {
        About GetCurrent();
    }
}
