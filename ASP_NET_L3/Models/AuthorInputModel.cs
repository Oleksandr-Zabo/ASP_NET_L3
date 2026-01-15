using System.ComponentModel.DataAnnotations;

namespace ASP_NET_L3.Models
{
    public class AuthorInputModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
