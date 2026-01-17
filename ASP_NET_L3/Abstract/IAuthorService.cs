using ASP_NET_L3.Models;

namespace ASP_NET_L3.Abstract
{
    public interface IAuthorService
    {
        (bool Success, string ErrorMessage) Save(AuthorDTO authorDto);
        AuthorDTO GetById(int id);
        List<AuthorDTO> GetAll();
        List<AuthorDTO> GetAuthorsWithBookCount();
    }
}
