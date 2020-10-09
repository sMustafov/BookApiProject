namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using BookApiProject.Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    public class ReviewerRepository : IReviewerRepository
    {
        private BookDbContext reviewerContext;
        public ReviewerRepository(BookDbContext reviewerContext)
        {
            this.reviewerContext = reviewerContext;
        }
        public Reviewer GetReviewer(int reviewerId)
        {
            return this.reviewerContext.Reviewers
                    .Where(r => r.Id == reviewerId)
                    .FirstOrDefault();
        }
        public Reviewer GetReviewerOfAReview(int reviewId)
        {
            var reviewerId = this.reviewerContext.Reviews
                    .Where(r => r.Id == reviewId)
                    .Select(rr => rr.Reviewer.Id)
                    .FirstOrDefault();
            
            return this.reviewerContext.Reviewers
                .Where(r => r.Id == reviewerId)
                .FirstOrDefault();
        }
        public ICollection<Reviewer> GetReviewers()
        {
            return this.reviewerContext.Reviewers
                    .OrderBy(r => r.LastName)
                    .ToList();
        }
        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return this.reviewerContext.Reviews
                    .Where(r => r.Reviewer.Id == reviewerId)
                    .ToList();
        }
        public bool ReviewerExists(int reviewerId)
        {
            return this.reviewerContext.Reviewers
                    .Any(r => r.Id == reviewerId);
        }
    }
}
