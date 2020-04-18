using System.Collections.Generic;
using System.Linq;
using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.Shared.Repository;

namespace BethanysPieShopHRM.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public Employee AddEmployee(Employee employee)
        {
            var addedEmployee = _context.Employees.Add(employee);
            _context.SaveChanges();

            return addedEmployee.Entity;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var foundEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (foundEmployee == null)
                return null;

            foundEmployee.FirstName = employee.FirstName;
            foundEmployee.LastName = employee.LastName;
            foundEmployee.BirthDate = employee.BirthDate;
            foundEmployee.Email = employee.Email;
            foundEmployee.Street = employee.Street;
            foundEmployee.Zip = employee.Zip;
            foundEmployee.City = employee.City;
            foundEmployee.CountryId = employee.CountryId;
            foundEmployee.PhoneNumber = employee.PhoneNumber;
            foundEmployee.Smoker = employee.Smoker; 
            foundEmployee.MaritalStatus = employee.MaritalStatus;
            foundEmployee.Gender = employee.Gender;
            foundEmployee.Comment = employee.Comment;
            foundEmployee.JoinedDate = employee.JoinedDate;
            foundEmployee.ExitDate = employee.ExitDate;
            foundEmployee.JobCategoryId = employee.JobCategoryId;
            foundEmployee.Latitude = employee.Latitude;
            foundEmployee.Longitude = employee.Longitude;

            _context.SaveChanges();

            return foundEmployee;
        }

        public void DeleteEmployee(int id)
        {
            var foundEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (foundEmployee == null)
                return;

            _context.Employees.Remove(foundEmployee);
            _context.SaveChanges();
        }
    }
}
