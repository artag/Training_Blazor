using System.Collections.Generic;
using System.Linq;
using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.Shared.Repository;

namespace BethanysPieShopHRM.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _context.Countries;
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.CountryId == id);
        }
    }
}
