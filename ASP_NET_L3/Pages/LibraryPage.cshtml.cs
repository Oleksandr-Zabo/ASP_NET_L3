using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_L3.Pages
{
    public class LibraryPageModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public LibraryPageModel(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public string SearchTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterAuthorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "Title";

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "ASC";

        // Data lists
        public List<AuthorDTO> Authors { get; set; }
        public List<BookDTO> Books { get; set; }
        public List<Author> AllAuthors { get; set; }

        public void OnGet()
        {
            // Get all authors with book count
            var allAuthors = _authorRepository.GetAll();
            var allBooks = _bookRepository.GetAll();

            Authors = allAuthors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate.ToShortDateString(),
                BookCount = allBooks.Count(b => b.AuthorId == a.Id)
            }).ToList();

            // Get books with filtering
            var booksQuery = allBooks.AsQueryable();

            // Filter by title
            if (!string.IsNullOrWhiteSpace(SearchTitle))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(SearchTitle, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by author
            if (FilterAuthorId.HasValue && FilterAuthorId.Value > 0)
            {
                booksQuery = booksQuery.Where(b => b.AuthorId == FilterAuthorId.Value);
            }

            // Sorting
            booksQuery = SortBy switch
            {
                "Year" => SortOrder == "DESC" ? booksQuery.OrderByDescending(b => b.PublishYear) : booksQuery.OrderBy(b => b.PublishYear),
                _ => SortOrder == "DESC" ? booksQuery.OrderByDescending(b => b.Title) : booksQuery.OrderBy(b => b.Title)
            };

            Books = booksQuery.Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublishYear = b.PublishYear,
                Price = b.Price,
                AuthorFullName = allAuthors.FirstOrDefault(a => a.Id == b.AuthorId) != null
                    ? $"{allAuthors.First(a => a.Id == b.AuthorId).FirstName} {allAuthors.First(a => a.Id == b.AuthorId).LastName}"
                    : "Unknown"
            }).ToList();

            // For dropdown
            AllAuthors = allAuthors;
        }
    }
}
