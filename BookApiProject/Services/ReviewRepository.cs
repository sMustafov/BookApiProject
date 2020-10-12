namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using BookApiProject.Services.Interfaces;
    using Microsoft.EntityFrameworkCore.Internal;
    using System.Collections.Generic;
    using System.Linq;

    public class ReviewRepository : IReviewRepository
    {
        private BookDbContext reviewContext;
        public ReviewRepository(BookDbContext reviewContext)
        {
            this.reviewContext = reviewContext;
        }

        public bool CreateReview(Review review)
        {
            this.reviewContext.Add(review);
            
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            this.reviewContext.Remove(review);
            
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            this.reviewContext.RemoveRange(reviews);
            
            return Save();
        }

        public Book GetBookOfAReview(int reviewId)
        {
            var bookId = this.reviewContext.Reviews
                    .Where(r => r.Id == reviewId)
                    .Select(b => b.Book.Id)
                    .FirstOrDefault();

            return this.reviewContext.Books
                    .Where(b => b.Id == bookId)
                    .FirstOrDefault();
        }
        public Review GetReview(int reviewId)
        {
            return this.reviewContext.Reviews
                    .Where(r => r.Id == reviewId)
                    .FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return this.reviewContext.Reviews
                    .OrderBy(r => r.Rating)
                    .ToList();
        }

        public ICollection<Review> GetReviewsOfABook(int bookId)
        {
            return this.reviewContext.Reviews
                    .Where(b => b.Book.Id == bookId)
                    .ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return this.reviewContext.Reviews
                    .Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = this.reviewContext.SaveChanges();

            if (saved < 0)
            {
                return false;
            }

            return true;
        }

        public bool UpdateReview(Review review)
        {
            this.reviewContext.Update(review);
            
            return Save();
        }
    }
}
