using BE.Concrete;

namespace BLL.Abstract
{
    public interface INewsLetterService : IGenericService<NewsLetter>
    {
        /// <summary>Trägt eine E-Mail-Adresse ein; false, wenn sie schon existiert.</summary>
        bool Subscribe(string mail);
    }
}
