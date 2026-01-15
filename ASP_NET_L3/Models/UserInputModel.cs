using System.ComponentModel.DataAnnotations;

namespace ASP_NET_L3.Models
{
    public class UserInputModel
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

    }
}
