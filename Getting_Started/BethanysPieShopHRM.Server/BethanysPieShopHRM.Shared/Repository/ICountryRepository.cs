using System.Collections.Generic;

namespace BethanysPieShopHRM.Shared.Repository
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountries();
        Country GetCountry(int id);
    }
}
