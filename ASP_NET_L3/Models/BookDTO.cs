using System.ComponentModel.DataAnnotations;

namespace ASP_NET_L3.Models
{
    public class BookDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Title { get; set; }

        [Required]
        [RegularExpression(@"^978-\d{10}$", ErrorMessage = "ISBN must be in format 978-XXXXXXXXXX")]
        public string ISBN { get; set; }

        [Required]
        [Range(1450, 2026, ErrorMessage = "Publish year must be between 1450 and current year")]
        public int PublishYear { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal? Price { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public string AuthorFullName { get; set; }
    }
}
