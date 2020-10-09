namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using BookApiProject.Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext authorContext;
        public AuthorRepository(BookDbContext authorContext)
        {
            this.authorContext = authorContext;
        }
        public bool AuthorExists(int authorId)
        {
            return this.authorContext.Authors
                    .Any(a => a.Id == authorId);
        }

        public Author GetAuthor(int authorId)
        {
            return this.authorContext.Authors
                    .Where(a => a.Id == authorId)
                    .FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return this.authorContext.Authors
                    .OrderBy(a => a.LastName)
                    .ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return this.authorContext.BookAuthors
                    .Where(b => b.Book.Id == bookId)
                    .Select(a => a.Author)
                    .ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return this.authorContext.BookAuthors
                    .Where(a => a.Author.Id == authorId)
                    .Select(b => b.Book)
                    .ToList();
        }
    }
}
