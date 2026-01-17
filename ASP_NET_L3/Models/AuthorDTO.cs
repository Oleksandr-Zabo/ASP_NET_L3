using System.ComponentModel.DataAnnotations;

namespace ASP_NET_L3.Models
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        public string BirthDate { get; set; }

        public int BookCount { get; set; }
    }
}