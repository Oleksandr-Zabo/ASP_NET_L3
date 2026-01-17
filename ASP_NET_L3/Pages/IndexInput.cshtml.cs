using ASP_NET_L3.Abstract;
using ASP_NET_L3.DAL;
using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;
using ASP_NET_L3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ASP_NET_L3.Pages
{
    public class IndexInputModel : PageModel
    {
        [BindProperty]
        public UserDTO User { get; set; }

        private readonly IUserService _userService;

        public IndexInputModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
            User = _userService.GetLastUser();

        }
        public void OnPostSave()
        {

            _userService.Save(User);

            this.OnGet();

        }
        public void OnPostDelete()
        {
            User.FirstName = string.Empty;
            User.LastName = string.Empty;
            User.CreatedDate = string.Empty;
        }
    }
}
