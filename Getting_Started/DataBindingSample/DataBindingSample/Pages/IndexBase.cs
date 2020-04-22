using System.Threading.Tasks;
using CommonData;
using Microsoft.AspNetCore.Components;

namespace DataBindingSample.Pages
{
    public class IndexBase : ComponentBase
    {
        public Employee Employee { get; set; }

        protected override Task OnInitializedAsync()
        {
            Employee = new Employee
            {
                FirstName = "Bethany",
                LastName = "Smith",
            };

            return base.OnInitializedAsync();
        }

        public void Button_Click()
        {
            Employee.FirstName = "Gill";
        }
    }
}
