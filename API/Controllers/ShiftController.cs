using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        public IShiftDAO _shiftDAO;

        public ShiftController(IShiftDAO shiftDAO)
        {
            _shiftDAO = shiftDAO;
        }

        // POST: api/Shift
        [HttpPost]
        public ActionResult AddShift([FromBody] Shift shift)
        {
            try
            {
                _shiftDAO.AddShift(shift);
                return Ok("Shift added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // PUT: api/Shift/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateShift(int id, [FromBody] Shift shift)
        {
            try
            {
                bool updated = _shiftDAO.UpdateShift(shift);
                if (updated)
                {
                    return Ok("Shift updated successfully.");
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

        // GET: api/Shift
        [HttpGet]
        public ActionResult<IEnumerable<Shift>> GetShifts()
        {
            try
            {
                var shifts = _shiftDAO.GetShifts();
                return Ok(shifts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Shift/{id}
        [HttpGet("{id}")]
        public ActionResult<Shift> GetShiftById(int id)
        {
            try
            {
                var shift = _shiftDAO.GetShiftById(id);
                if (shift == null)
                {
                    return NotFound($"ID {id} not found.");
                }
                return Ok(shift);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
