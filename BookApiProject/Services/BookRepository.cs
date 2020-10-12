namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using BookApiProject.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = this.bookContext.Authors
                    .Where(a => authorsId.Contains(a.Id))
                    .ToList();
            var categories = this.bookContext.Categories
                    .Where(c => categoriesId.Contains(c.Id))
                    .ToList();

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };

                this.bookContext.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };

                this.bookContext.Add(bookCategory);
            }

            this.bookContext.Add(book);

            return Save();
        }

        public bool DeleteBook(Book book)
        {
            this.bookContext.Remove(book);
            
            return Save();
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

        public bool Save()
        {
            var saved = this.bookContext.SaveChanges();

            if (saved < 0)
            {
                return false;
            }

            return true;
        }

        public bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = this.bookContext.Authors
                    .Where(a => authorsId.Contains(a.Id))
                    .ToList();

            var categories = this.bookContext.Categories
                    .Where(c => categoriesId.Contains(c.Id))
                    .ToList();


            var bookAuthorsToDelete = this.bookContext.BookAuthors
                    .Where(b => b.BookId == book.Id);

            var bookCategoriesToDelete = this.bookContext.BookCategories
                    .Where(b => b.BookId == book.Id);

            this.bookContext.RemoveRange(bookAuthorsToDelete);
            this.bookContext.RemoveRange(bookCategoriesToDelete);

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };

                this.bookContext.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };

                this.bookContext.Add(bookCategory);
            }

            this.bookContext.Update(book);

            return Save();
        }
    }
}
