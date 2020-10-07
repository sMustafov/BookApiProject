namespace BookApiProject
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using System;
    using System.Collections.Generic;
    public static class DbSeedingClass
    {
        public static void SeedDataContext(this BookDbContext context)
        {
            var booksAuthors = new List<BookAuthor>()
            {
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "123",
                        Title = "Book first",
                        DatePublished = new DateTime(1990, 1, 1),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Action"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { Headline = "Awesome Book", ReviewText = "Reviewing", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Ivan", LastName = "Ivan" } },
                            new Review { Headline = "Terrible Book", ReviewText = "Reviewing", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Peho", LastName = "Pesho" } },
                            new Review { Headline = "Decent Book", ReviewText = "Not a bad", Rating = 3,
                                Reviewer = new Reviewer(){ FirstName = "Gosho", LastName = "Gosho" } }
                        }
                    },
                    Author = new Author()
                    {
                        FirstName = "First",
                        LastName = "Last",
                        Country = new Country()
                        {
                            Name = "BG"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "1234",
                        Title = "Second book",
                        DatePublished = new DateTime(1899, 1, 1),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Western"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { Headline = "Awesome Book", ReviewText = "Reviewing Book", Rating = 4,
                                Reviewer = new Reviewer(){ FirstName = "Gosho", LastName = "Pesho" } }
                        }
                    },
                    Author = new Author()
                    {
                        FirstName = "Name",
                        LastName = "Namov",
                        Country = new Country()
                        {
                            Name = "UK"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "12345",
                        Title = "Last Book",
                        DatePublished = new DateTime(200, 2, 2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Action"}},
                            new BookCategory { Category = new Category() { Name = "Sport"}}
                        }
                    },
                    Author = new Author()
                    {
                        FirstName = "Ivan",
                        LastName = "Goshov",
                        Country = new Country()
                        {
                            Name = "FR"
                        }
                    }
                }
            };

            context.BookAuthors.AddRange(booksAuthors);
            context.SaveChanges();
        }
    }
}
