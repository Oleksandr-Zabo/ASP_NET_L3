using ASP_NET_L3.Abstract;
using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;

namespace ASP_NET_L3.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public List<UserDTO> GetAll()
        {
            var users = _userRepository.GetAll();
            var usersDto = new List<UserDTO>();
            foreach (var user in users) {
                usersDto.Add(MapToDto(user));
            }
            return usersDto;
        }

        public bool Save(UserDTO userDto)
        {
            var user = new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                CreatedDate = userDto.CreatedDate != null ? DateTime.Parse(userDto.CreatedDate) : DateTime.Now,
            };
            return _userRepository.AddUser(user);
        }

        public UserDTO GetLastUser() {
            var user = _userRepository.GetAll().LastOrDefault();
            return MapToDto(user);
        }

        public UserDTO GetFirstUser()
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            return MapToDto(user);
        }

        private UserDTO MapToDto(User user) {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = user.CreatedDate.ToShortDateString(),
            };
        }
    }
}
