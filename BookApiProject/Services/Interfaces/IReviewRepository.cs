﻿namespace BookApiProject.Services.Interfaces
{
    using BookApiProject.Models;
    using System.Collections.Generic;
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfABook(int bookId);
        Book GetBookOfAReview(int reviewId);
        bool ReviewExists(int reviewId);
    }
}