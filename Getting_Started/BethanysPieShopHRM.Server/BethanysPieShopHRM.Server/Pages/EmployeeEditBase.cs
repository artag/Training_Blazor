using System;
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

        protected string Message { get; private set; } = string.Empty;

        protected string StatusClass { get; private set; } = string.Empty;

        protected bool Saved { get; private set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            Countries = await CountryDataService.GetAllCountries();
            JobCategories = await JobCategoryDataService.GetAllJobCategories();

            int.TryParse(EmployeeId, out var employeeId);

            if (employeeId == 0)
            {
                // New employee
                Employee = new Employee
                {
                    CountryId = 1,
                    JobCategoryId = 1,
                    BirthDate = DateTime.Now,
                    JoinedDate = DateTime.Now
                };
            }
            else
            {
                Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
            }

            CountryId = Employee.CountryId.ToString();
            JobCategoryId = Employee.JobCategoryId.ToString();
        }

        protected async Task HandleValidSubmit()
        {
            Employee.CountryId = int.Parse(CountryId);
            Employee.JobCategoryId = int.Parse(JobCategoryId);

            if (Employee.EmployeeId == 0)
            {
                await SubmitAddingNewEmployee();
            }
            else
            {
                await SubmitUpdateExistingEmployee();
            }
        }

        private async Task SubmitAddingNewEmployee()
        {
            var addedEmployee = await EmployeeDataService.AddEmployee(Employee);
            if (addedEmployee != null)
            {
                StatusClass = "alert-success";
                Message = "New employee added successfully";
                Saved = true;
            }
            else
            {
                StatusClass = "alert-danger";
                Message = "Something went wrong adding the new employee. Please try again.";
                Saved = false;
            }
        }

        private async Task SubmitUpdateExistingEmployee()
        {
            await EmployeeDataService.UpdateEmployee(Employee);

            StatusClass = "alert-success";
            Message = "Employee updated successfully";
            Saved = true;
        }
    }
}
