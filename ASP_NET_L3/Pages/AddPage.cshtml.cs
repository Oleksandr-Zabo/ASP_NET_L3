using ASP_NET_L3.Abstract;
using ASP_NET_L3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_NET_L3.Pages
{
    public class AddPageModel : PageModel
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public AddPageModel(IAuthorService authorService, IBookService bookService)
        {
            _authorService = authorService;
            _bookService = bookService;
        }

        // For Author
        [BindProperty]
        public AuthorDTO Author { get; set; }

        // For Book
        [BindProperty]
        public BookDTO Book { get; set; }

        // Success messages
        [TempData]
        public string SuccessMessage { get; set; }

        // Error messages
        [TempData]
        public string ErrorMessage { get; set; }

        // For dropdown
        public List<AuthorDTO> AllAuthors { get; set; }

        public void OnGet()
        {
            AllAuthors = _authorService.GetAll();
        }

        public IActionResult OnPostSaveAuthor()
        {
            // Load authors for dropdown in case of error
            AllAuthors = _authorService.GetAll();

            // Save author
            var result = _authorService.Save(Author);
            
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return Page();
            }

            SuccessMessage = $"Author '{Author.FirstName} {Author.LastName}' added successfully!";
            return RedirectToPage();
        }

        public IActionResult OnPostSaveBook()
        {
            // Load authors for dropdown in case of error
            AllAuthors = _authorService.GetAll();

            // Save book through service (validation inside)
            var result = _bookService.Save(Book);
            
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return Page();
            }

            SuccessMessage = $"Book '{Book.Title}' added successfully!";
            return RedirectToPage();
        }
    }
}
