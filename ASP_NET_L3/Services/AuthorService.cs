using ASP_NET_L3.Abstract;
using ASP_NET_L3.DAL.Abstracts;
using ASP_NET_L3.DAL.Entities;
using ASP_NET_L3.Models;

namespace ASP_NET_L3.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        public (bool Success, string ErrorMessage) Save(AuthorDTO authorDto)
        {
            // Validate if author already exists
            var birthDate = authorDto.BirthDate != null ? DateTime.Parse(authorDto.BirthDate) : DateTime.Now;
            
            // Check on service level
            if (AuthorExists(authorDto.FirstName, authorDto.LastName, birthDate))
            {
                return (false, $"Author '{authorDto.FirstName} {authorDto.LastName}' with birth date {birthDate:yyyy-MM-dd} already exists!");
            }

            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                BirthDate = birthDate
            };

            _authorRepository.AddAuthor(author);
            return (true, null);
        }

        public AuthorDTO GetById(int id)
        {
            var author = _authorRepository.GetById(id);
            return MapToDTO(author);
        }

        public List<AuthorDTO> GetAll()
        {
            return _authorRepository.GetAll()
                .Select(MapToDTO)
                .ToList();
        }

        public List<AuthorDTO> GetAuthorsWithBookCount()
        {
            var allAuthors = _authorRepository.GetAll();
            var allBooks = _bookRepository.GetAll();

            return allAuthors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthDate = a.BirthDate.ToShortDateString(),
                BookCount = allBooks.Count(b => b.AuthorId == a.Id)
            }).ToList();
        }

        private bool AuthorExists(string firstName, string lastName, DateTime birthDate)
        {
            var allAuthors = _authorRepository.GetAll();
            
            return allAuthors.Any(a =>
                a.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase) &&
                a.BirthDate.Date == birthDate.Date);
        }

        private AuthorDTO MapToDTO(Author author)
        {
            if (author == null)
            {
                return null;
            }

            return new AuthorDTO
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate.ToShortDateString()
            };
        }
    }
}
