using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeDAO _employeeDAO;

        public EmployeeController(IEmployeeDAO employeeDAO) 
        {
            _employeeDAO = employeeDAO;
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> employees = _employeeDAO.GetAllEmployees();
            return View(employees);
        }

        public IActionResult Create() 
        {
            return View(new Employee());
        }

        [HttpPost]
        public IActionResult Create(Employee employee) 
        {
            try
            {
                if (!ModelState.IsValid) { throw new InvalidDataException(); }
                _employeeDAO.AddEmployee(employee);
                return RedirectToAction("Index");
            }
            catch 
            { 
                return View(); 
            }
        }
    }
}
