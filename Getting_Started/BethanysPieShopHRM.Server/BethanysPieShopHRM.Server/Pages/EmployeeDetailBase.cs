using System.Collections.Generic;
using System.Threading.Tasks;
using BethanysPieShopHRM.ComponentsLibrary.Map;
using BethanysPieShopHRM.Server.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Server.Pages
{
    public class EmployeeDetailBase : ComponentBase
    {
        public IEnumerable<Employee> Employees { get; set; }

        [Parameter]
        public string EmployeeId { get; set; }

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public Employee Employee { get; private set; } = new Employee();

        public IEnumerable<Marker> MapMarkers { get; set; } = new List<Marker>();

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));

            MapMarkers = new List<Marker>
            {
                new Marker
                {
                    Description = $"{Employee.FirstName} {Employee.LastName}",
                    ShowPopup = false,
                    X = Employee.Longitude,
                    Y = Employee.Latitude,
                }
            };
        }
    }
}
