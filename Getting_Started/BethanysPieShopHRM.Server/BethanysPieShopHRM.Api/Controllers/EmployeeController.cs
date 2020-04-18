using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.Shared.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_repository.GetAllEmployees());
        }

        // GET: api/<controller>/<id>
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            return Ok(_repository.GetEmployee(id));
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest();

            if (string.IsNullOrEmpty(employee.FirstName))
                ModelState.AddModelError("FirstName", "The first name shouldn't be empty");

            if (string.IsNullOrEmpty(employee.LastName))
                ModelState.AddModelError("LastName", "The last name shouldn't be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEmployee = _repository.AddEmployee(employee);

            return Created("employee", createdEmployee);
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest();

            if (string.IsNullOrEmpty(employee.FirstName))
                ModelState.AddModelError("FirstName", "The first name shouldn't be empty");

            if (string.IsNullOrEmpty(employee.LastName))
                ModelState.AddModelError("LastName", "The last name shouldn't be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeToUpdate = _repository.GetEmployee(employee.EmployeeId);
            if (employeeToUpdate == null)
                return NotFound();

            _repository.UpdateEmployee(employee);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (id == 0)
                return BadRequest();

            var employeeToDelete = _repository.GetEmployee(id);
            if (employeeToDelete == null)
                return NotFound();

            _repository.DeleteEmployee(id);

            return NoContent();
        }
    }
}
