namespace BookApiProject.Controllers
{
    using BookApiProject.Dtos;
    using BookApiProject.Models;
    using BookApiProject.Services;
    using BookApiProject.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private IBookRepository bookRepository;
        private IAuthorRepository authorRepository;
        private ICategoryRepository categoryRepository;
        private IReviewRepository reviewRepository;

        public BooksController(
            IBookRepository bookRepository, 
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IReviewRepository reviewRepository)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.categoryRepository = categoryRepository;
            this.reviewRepository = reviewRepository;
        }

        // api/books
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooks()
        {
            var books = this.bookRepository.GetBooks();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }

            return Ok(booksDto);
        }

        // api/books/bookId
        [HttpGet("{bookId}", Name = "GetBook")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(int bookId)
        {
            if (!this.bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var book = this.bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }

        // api/books/isbn/bookIsbn
        [HttpGet("ISBN/{bookIsbn}")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(string bookIsbn)
        {
            if (!this.bookRepository.BookExists(bookIsbn))
            {
                return NotFound();
            }

            var book = this.bookRepository.GetBook(bookIsbn);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                DatePublished = book.DatePublished
            };

            return Ok(bookDto);
        }

        // api/books/bookId/rating
        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookRating(int bookId)
        {
            if (!this.bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var rating = this.bookRepository.GetBookRating(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        // api/books?authId=1&authId=2&catId=1&catId=2
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Book))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateBook([FromQuery] List<int> authId, [FromQuery] List<int> catId,
                                        [FromBody] Book bookToCreate)
        {
            var statusCode = ValidateBook(authId, catId, bookToCreate);

            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            if (!this.bookRepository.CreateBook(authId, catId, bookToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the book " +
                                            $"{bookToCreate.Title}");
                
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new { bookId = bookToCreate.Id }, bookToCreate);
        }

        // api/books/bookId?authId=1&authId=2&catId=1&catId=2
        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId, [FromQuery] List<int> authId, [FromQuery] List<int> catId,
                                        [FromBody] Book bookToUpdate)
        {
            var statusCode = ValidateBook(authId, catId, bookToUpdate);

            if (bookId != bookToUpdate.Id)
            {
                return BadRequest();
            }

            if (!this.bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            if (!this.bookRepository.UpdateBook(authId, catId, bookToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating the book " +
                                            $"{bookToUpdate.Title}");
                
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // api/books/bookId
        [HttpDelete("{bookId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteBook(int bookId)
        {
            if (!this.bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var reviewsToDelete = this.reviewRepository.GetReviewsOfABook(bookId);
            var bookToDelete = this.bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong deleting reviews");
                
                return StatusCode(500, ModelState);
            }

            if (!this.bookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting book {bookToDelete.Title}");
                
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        private StatusCodeResult ValidateBook(List<int> authId, List<int> catId, Book book)
        {
            if (book == null || authId.Count() <= 0 || catId.Count() <= 0)
            {
                ModelState.AddModelError("", "Missing book, author, or category");
                
                return BadRequest();
            }

            if (this.bookRepository.IsDuplicateIsbn(book.Id, book.Isbn))
            {
                ModelState.AddModelError("", "Duplicate ISBN");
                
                return StatusCode(422);
            }

            foreach (var id in authId)
            {
                if (!this.authorRepository.AuthorExists(id))
                {
                    ModelState.AddModelError("", "Author Not Found");
                 
                    return StatusCode(404);
                }
            }

            foreach (var id in catId)
            {
                if (!this.categoryRepository.CategoryExists(id))
                {
                    ModelState.AddModelError("", "Category Not Found");
                    
                    return StatusCode(404);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Critical Error");
                
                return BadRequest();
            }

            return NoContent();
        }
    }
}
