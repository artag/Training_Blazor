using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;

namespace BethanysPieShopHRM.Server.Services
{
    public class JobCategoryDataService : IJobCategoryDataService
    {
        private readonly HttpClient _httpClient;

        public JobCategoryDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<JobCategory>> GetAllJobCategories()
        {
            var jobCategories = await _httpClient.GetStreamAsync("api/jobcategory");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = await JsonSerializer.DeserializeAsync<IEnumerable<JobCategory>>(jobCategories, options);

            return data;
        }

        public async Task<JobCategory> GetJobCategory(int id)
        {
            var jobCategory = await _httpClient.GetStreamAsync($"api/jobcategory/{id}");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = await JsonSerializer.DeserializeAsync<JobCategory>(jobCategory, options);

            return data;
        }
    }
}
