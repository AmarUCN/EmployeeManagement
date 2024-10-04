using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftEmployeesController : ControllerBase
    {
        public IShiftEmployeesDAO _shiftEmployeesDAO;

        public ShiftEmployeesController(IShiftEmployeesDAO shiftEmployeesDAO)
        {
            _shiftEmployeesDAO = shiftEmployeesDAO;
        }

        // POST: api/ShiftEmployees
        [HttpPost]
        public ActionResult AddShiftEmployees([FromBody] ShiftEmployees shiftEmployees)
        {
            try
            {
                _shiftEmployeesDAO.AddShiftEmployees(shiftEmployees);
                return Ok("Added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // PUT: api/ShiftEmployees/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateShiftEmployees(int id, [FromBody] ShiftEmployees shiftEmployees)
        {
            try
            {
                bool updated = _shiftEmployeesDAO.UpdateShiftEmployees(shiftEmployees);
                if (updated)
                {
                    return Ok("Updated successfully.");
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

        // GET: api/ShiftEmployees
        [HttpGet]
        public ActionResult<IEnumerable<ShiftEmployees>> GetShiftEmployeess()
        {
            try
            {
                var shiftEmployees = _shiftEmployeesDAO.GetShiftEmployeess();
                return Ok(shiftEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/ShiftEmployees/{id}
        [HttpGet("{id}")]
        public ActionResult<ShiftEmployees> GetShiftEmployeesById(int id)
        {
            try
            {
                var shiftEmployees = _shiftEmployeesDAO.GetShiftEmployeesById(id);
                if (shiftEmployees == null)
                {
                    return NotFound($"ID {id} not found.");
                }
                return Ok(shiftEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
