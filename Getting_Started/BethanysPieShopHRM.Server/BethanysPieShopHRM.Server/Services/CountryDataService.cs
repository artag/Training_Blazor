using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;

namespace BethanysPieShopHRM.Server.Services
{
    public class CountryDataService : ICountryDataService
    {
        private readonly HttpClient _httpClient;

        public CountryDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var countries = await _httpClient.GetStreamAsync("api/country");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = await JsonSerializer.DeserializeAsync<IEnumerable<Country>>(countries, options);

            return data;
        }

        public async Task<Country> GetCountry(int id)
        {
            var country = await _httpClient.GetStreamAsync($"api/country/{id}");
            var option = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
            var data = await JsonSerializer.DeserializeAsync<Country>(country, option);

            return data;
        }
    }
}
