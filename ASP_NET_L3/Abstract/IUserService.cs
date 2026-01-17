using ASP_NET_L3.Models;

namespace ASP_NET_L3.Abstract
{
    public interface IUserService
    {
        bool Save(UserDTO user);

        List<UserDTO> GetAll();

        UserDTO GetLastUser();

        UserDTO GetFirstUser();
    }
}
