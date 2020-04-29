using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;

namespace BethanysPieShopHRM.Server.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private const string MediaType = "application/json";

        private readonly HttpClient _httpClient;

        public EmployeeDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var content = JsonSerializer.Serialize(employee);
            var employeeJson = new StringContent(content, Encoding.UTF8, MediaType);

            var response = await _httpClient.PostAsync("api/employee", employeeJson);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Employee>(contentStream);
            }

            return null;
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync($"api/employee/{id}");
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = await _httpClient.GetStreamAsync(@"api/employee");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>(
                employees, options);

            return data;
        }

        public async Task<Employee> GetEmployeeDetails(int id)
        {
            var employee = await _httpClient.GetStreamAsync($"api/employee/{id}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = await JsonSerializer.DeserializeAsync<Employee>(
                employee, options);

            return data;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var content = JsonSerializer.Serialize(employee);
            var employeeJson = new StringContent(content, Encoding.UTF8, MediaType);

            await _httpClient.PutAsync("api/employee", employeeJson);
        }
    }
}
