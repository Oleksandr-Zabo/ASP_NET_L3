namespace ASP_NET_L3.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishYear { get; set; }
        public decimal? Price { get; set; }
        public string AuthorFullName { get; set; }
    }
}
