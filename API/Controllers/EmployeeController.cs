using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public IEmployeeDAO _employeeDAO;

        public EmployeeController(IEmployeeDAO employeeDAO)
        {
            _employeeDAO = employeeDAO;
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult AddEmployee([FromBody] Employee employee)
        {
            try
            {
                _employeeDAO.AddEmployee(employee);
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                bool isDeleted = _employeeDAO.DeleteEmployee(id);
                if (isDeleted)
                {
                    return Ok("Employee deleted successfully.");
                }
                else
                {
                    return NotFound($"ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeDAO.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            try
            {
                var employee = _employeeDAO.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound($"ID {id} not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
