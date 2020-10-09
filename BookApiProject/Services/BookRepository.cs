namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using BookApiProject.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BookRepository : IBookRepository
    {
        private BookDbContext bookContext;
        public BookRepository(BookDbContext bookContext)
        {
            this.bookContext = bookContext;
        }
        public bool BookExists(int bookId)
        {
            return this.bookContext.Books
                    .Any(b => b.Id == bookId);
        }

        public bool BookExists(string bookIsbn)
        {
            return this.bookContext.Books
                    .Any(b => b.Isbn == bookIsbn);
        }

        public Book GetBook(int bookId)
        {
            return this.bookContext.Books
                    .Where(b => b.Id == bookId)
                    .FirstOrDefault();
        }

        public Book GetBook(string bookIsbn)
        {
            return this.bookContext.Books
                    .Where(b => b.Isbn == bookIsbn)
                    .FirstOrDefault();
        }

        public decimal GetBookRating(int bookId)
        {
            var reviews = this.bookContext.Reviews
                                .Where(r => r.Book.Id == bookId);

            if (reviews.Count() <= 0)
                return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public ICollection<Book> GetBooks()
        {
            return this.bookContext.Books
                    .OrderBy(b => b.Title)
                    .ToList();
        }

        public bool IsDuplicateIsbn(int bookId, string bookIsbn)
        {
            var book = this.bookContext.Books
                    .Where(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id != bookId)
                    .FirstOrDefault();
          
            if(book == null)
            {
                return false;
            }

            return true;
        }
    }
}
