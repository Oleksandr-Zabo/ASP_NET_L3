using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_L3.Pages
{
    public class AddPageModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public AddPageModel(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        // For Author
        [BindProperty]
        public AuthorInputModel AuthorInput { get; set; }

        // For Book
        [BindProperty]
        public BookInputModel BookInput { get; set; }

        // Success messages
        [TempData]
        public string SuccessMessage { get; set; }

        // Error messages
        [TempData]
        public string ErrorMessage { get; set; }

        // For dropdown
        public List<Author> AllAuthors { get; set; }

        public void OnGet()
        {
            AllAuthors = _authorRepository.GetAll();
        }

        public IActionResult OnPostSaveAuthor()
        {
            // Load authors for dropdown in case of error
            AllAuthors = _authorRepository.GetAll();

            //// Clear validation errors for BookInput (we're not validating it here)
            //ModelState.Remove("BookInput.Title");
            //ModelState.Remove("BookInput.ISBN");
            //ModelState.Remove("BookInput.PublishYear");
            //ModelState.Remove("BookInput.AuthorId");

            //// Validate model state for AuthorInput only
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            // Check if author already exists
            if (_authorRepository.AuthorExists(AuthorInput.FirstName, AuthorInput.LastName, AuthorInput.BirthDate))
            {                ModelState.AddModelError(string.Empty, 
                    $"Author '{AuthorInput.FirstName} {AuthorInput.LastName}' with birth date {AuthorInput.BirthDate:yyyy-MM-dd} already exists!");
                return Page();
            }

            // Create and save author
            var author = new Author
            {
                FirstName = AuthorInput.FirstName,
                LastName = AuthorInput.LastName,
                BirthDate = AuthorInput.BirthDate,
            };

            _authorRepository.AddAuthor(author);

            SuccessMessage = $"Author '{author.FirstName} {author.LastName}' added successfully!";
            return RedirectToPage();
        }

        public IActionResult OnPostSaveBook()
        {
            // Load authors for dropdown in case of error
            AllAuthors = _authorRepository.GetAll();

            //// Clear validation errors for AuthorInput (we're not validating it here)
            //ModelState.Remove("AuthorInput.FirstName");
            //ModelState.Remove("AuthorInput.LastName");
            //ModelState.Remove("AuthorInput.BirthDate");

            //// Validate model state for BookInput only
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            // Check if book with same ISBN already exists
            if (_bookRepository.BookExistsByISBN(BookInput.ISBN))
            {

            }

            // Check if book with same title and author already exists
            if (_bookRepository.BookExistsByTitle(BookInput.Title, BookInput.AuthorId))
            {
                var author = _authorRepository.GetById(BookInput.AuthorId);
                ModelState.AddModelError(string.Empty, 
                    $"Book '{BookInput.Title}' by {author.FirstName} {author.LastName} already exists!");
                return Page();
            }

            // Create and save book
            var book = new Book
            {
                Title = BookInput.Title,
                ISBN = BookInput.ISBN,
                PublishYear = BookInput.PublishYear,
                Price = BookInput.Price,
                AuthorId = BookInput.AuthorId
            };

            _bookRepository.AddBook(book);

            SuccessMessage = $"Book '{book.Title}' added successfully!";
            return RedirectToPage();
        }
    }
}
