using ASP_NET_L3.Models;

namespace ASP_NET_L3.Abstract
{
    public interface IBookService
    {
        (bool Success, string ErrorMessage) Save(BookDTO bookDto);
        BookDTO GetById(int id);
        List<BookDTO> GetAll();
    }
}