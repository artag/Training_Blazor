using System.Collections.Generic;
using System.Threading.Tasks;
using BethanysPieShopHRM.Server.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Server.Pages
{
    public class EmployeeEditBase : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public ICountryDataService CountryDataService { get; set; }

        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Parameter]
        public string EmployeeId { get; set; }

        public IEnumerable<Country> Countries { get; set; } = new List<Country>();

        public IEnumerable<JobCategory> JobCategories { get; set; } = new List<JobCategory>();

        public Employee Employee { get; set; } = new Employee();

        protected string CountryId { get; set; } = string.Empty;

        protected string JobCategoryId { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
            Countries = await CountryDataService.GetAllCountries();
            JobCategories = await JobCategoryDataService.GetAllJobCategories();

            CountryId = Employee.CountryId.ToString();
            JobCategoryId = Employee.JobCategoryId.ToString();
        }
    }
}
