using ASP_NET_L3.Abstract;
using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;

namespace ASP_NET_L3.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public (bool Success, string ErrorMessage) Save(BookDTO bookDto)
        {
            // Check if book with same ISBN already exists
            if (_bookRepository.BookExistsByISBN(bookDto.ISBN))
            {
                return (false, $"Book with ISBN '{bookDto.ISBN}' already exists!");
            }

            // Check if book with same title and author already exists
            if (_bookRepository.BookExistsByTitle(bookDto.Title, bookDto.AuthorId))
            {
                var author = _authorRepository.GetById(bookDto.AuthorId);
                if (author != null)
                {
                    return (false, $"Book '{bookDto.Title}' by {author.FirstName} {author.LastName} already exists!");
                }
                else
                {
                    return (false, $"Book '{bookDto.Title}' already exists!");
                }
            }

            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                PublishYear = bookDto.PublishYear,
                Price = bookDto.Price,
                AuthorId = bookDto.AuthorId
            };

            _bookRepository.AddBook(book);
            return (true, null);
        }

        public BookDTO GetById(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return null;
            }

            var author = _authorRepository.GetById(book.AuthorId);
            return MapToDTO(book, author);
        }

        public List<BookDTO> GetAll()
        {
            var allBooks = _bookRepository.GetAll();
            var allAuthors = _authorRepository.GetAll();

            return allBooks.Select(b => MapToDTO(b, allAuthors.FirstOrDefault(a => a.Id == b.AuthorId))).ToList();
        }

        public List<BookDTO> GetBooksWithFilters(string searchTitle, int? filterAuthorId, string sortBy, string sortOrder)
        {
            var allBooks = _bookRepository.GetAll();
            var allAuthors = _authorRepository.GetAll();

            // Get books with filtering
            var booksQuery = allBooks.AsQueryable();

            // Filter by title
            if (!string.IsNullOrWhiteSpace(searchTitle))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchTitle, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by author
            if (filterAuthorId.HasValue && filterAuthorId.Value > 0)
            {
                booksQuery = booksQuery.Where(b => b.AuthorId == filterAuthorId.Value);
            }

            // Sorting
            booksQuery = sortBy switch
            {
                "Year" => sortOrder == "DESC" 
                    ? booksQuery.OrderByDescending(b => b.PublishYear) 
                    : booksQuery.OrderBy(b => b.PublishYear),
                _ => sortOrder == "DESC" 
                    ? booksQuery.OrderByDescending(b => b.Title) 
                    : booksQuery.OrderBy(b => b.Title)
            };

            return booksQuery.Select(b => MapToDTO(b, allAuthors.FirstOrDefault(a => a.Id == b.AuthorId))).ToList();
        }

        private BookDTO MapToDTO(Book book, Author author)
        {
            if (book == null)
            {
                return null;
            }

            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                PublishYear = book.PublishYear,
                Price = book.Price,
                AuthorId = book.AuthorId,
                AuthorFullName = author != null 
                    ? $"{author.FirstName} {author.LastName}" 
                    : "Unknown"
            };
        }
    }
}
