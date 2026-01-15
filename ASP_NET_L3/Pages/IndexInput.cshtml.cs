using ASP_NET_L3.DAL;
using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_L3.Pages
{
    public class IndexInputModel : PageModel
    {
        [BindProperty]
        public UserInputModel Input { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedDate { get; set; }

        private readonly IUserRepository _userRepository;

        public IndexInputModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
            var user = _userRepository.GetAll().LastOrDefault();
            FirstName = user.FirstName;
            LastName = user.LastName;
            CreatedDate = user.CreatedDate.ToShortDateString();
        }
        public void OnPostSave()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            var User = new DAL.Entities.User
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                CreatedDate = DateTime.Now,
            };

            _userRepository.AddUser(User);

            this.OnGet();

        }
        public void OnPostDelete()
        {
            FirstName = "";
        }
    }
}
