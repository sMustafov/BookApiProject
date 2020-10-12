namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using System.Collections.Generic;
    using System.Linq;
    public class CategoryRepository : ICategoryRepository
    {
        private BookDbContext categoryContext;
        public CategoryRepository(BookDbContext categoryContext)
        {
            this.categoryContext = categoryContext;
        }
        public bool CategoryExists(int categoryId)
        {
            return this.categoryContext.Categories
                    .Any(c => c.Id == categoryId);
        }
        public ICollection<Book> GetAllBooksForCategory(int categoryId)
        {
            return this.categoryContext.BookCategories
                    .Where(c => c.CategoryId == categoryId)
                    .Select(b => b.Book)
                    .ToList();
        }
        public ICollection<Category> GetCategories()
        {
            return this.categoryContext.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
        }
        public ICollection<Category> GetAllCategoriesForBook(int bookId)
        {
            return this.categoryContext.BookCategories
                    .Where(b => b.BookId == bookId)
                    .Select(c => c.Category)
                    .ToList();
        }
        public Category GetCategory(int categoryId)
        {
            return this.categoryContext.Categories
                    .Where(c => c.Id == categoryId)
                    .FirstOrDefault();
        }
        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = this.categoryContext.Categories
                    .Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id != categoryId)
                    .FirstOrDefault();

            if(category == null)
            {
                return false;
            }

            return true;
        }

        public bool CreateCategory(Category category)
        {
            this.categoryContext.Add(category);

            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            this.categoryContext.Update(category);

            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            this.categoryContext.Remove(category);

            return Save();
        }

        public bool Save()
        {
            var saved = this.categoryContext.SaveChanges();

            if (saved < 0)
            {
                return false;
            }

            return true;
        }
    }
}
