using System.Collections.Generic;
using System.Threading.Tasks;
using BethanysPieShopHRM.Server.Components;
using BethanysPieShopHRM.Server.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Server.Pages
{
    public class EmployeeOverviewBase : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        /// <summary>
        /// Компонент - диалог.
        /// </summary>
        protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        public async void AddEmployeeDialog_OnDialogClose()
        {
            Employees = await EmployeeDataService.GetAllEmployees();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            Employees = await EmployeeDataService.GetAllEmployees();
        }

        protected void QuickAddEmployee()
        {
            AddEmployeeDialog.Show();
        }
    }
}
