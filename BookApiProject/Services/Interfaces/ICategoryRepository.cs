namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using System.Collections.Generic;
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetAllCategoriesForBook(int bookId);
        ICollection<Book> GetAllBooksForCategory(int countryId);
        bool CategoryExists(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);
    }
}
