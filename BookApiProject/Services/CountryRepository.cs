namespace BookApiProject.Services
{
    using BookApiProject.Models;
    using BookApiProject.Service;
    using System.Collections.Generic;
    using System.Linq;
    public class CountryRepository : ICountryRepository
    {
        private BookDbContext countryContext;
        public CountryRepository(BookDbContext countryContext)
        {
            this.countryContext = countryContext;
        }
        public bool CountryExists(int countryId)
        {
            return this.countryContext.Countries
                    .Any(c => c.Id == countryId); 
        }
        public ICollection<Author> GetAuthorsFromCountry(int countryId)
        {
            return this.countryContext.Authors
                    .Where(c => c.Country.Id == countryId)
                    .ToList();
        }
        public ICollection<Country> GetCountries()
        {
            return this.countryContext.Countries
                    .OrderBy(c => c.Name)
                    .ToList();
        }
        public Country GetCountry(int countryId)
        {
            return this.countryContext.Countries
                    .Where(c => c.Id == countryId)
                    .FirstOrDefault();
        }
        public Country GetCountryOfAnAuthor(int authorId)
        {
            return this.countryContext.Authors
                    .Where(a => a.Id == authorId)
                    .Select(c => c.Country)
                    .FirstOrDefault();
        }
        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country = this.countryContext.Countries
                    .Where(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id != countryId)
                    .FirstOrDefault();

            if (country == null)
            {
                return false;
            }

            return true;
        }
    }
}
