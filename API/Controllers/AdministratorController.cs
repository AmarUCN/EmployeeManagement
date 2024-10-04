using API.DTO;
using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        public IAdministratorDAO _administratorDAO;

        public AdministratorController(IAdministratorDAO administratorDAO)
        {
            _administratorDAO = administratorDAO;
        }

        // POST: api/Administrator
        [HttpPost]
        public ActionResult<int> AddAdministrator([FromBody] Administrator administrator)
        {
            try
            {
                int newAdminId = _administratorDAO.AddAdministrator(administrator);
                return CreatedAtAction(nameof(GetById), new { id = newAdminId }, newAdminId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Administrator/{id}
        [HttpGet("{id}")]
        public ActionResult<Administrator> GetById(int id)
        {
            try
            {
                var administrator = _administratorDAO.GetById(id);
                if (administrator == null)
                {
                    return NotFound($"ID {id} not found");
                }
                return Ok(administrator);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // POST: api/Administrator/Login
        [HttpPost("Login")]
        public ActionResult<Administrator> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var administrator = _administratorDAO.Login(loginRequest.Email, loginRequest.Password);
                if (administrator == null)
                {
                    return Unauthorized("Invalid email or password");
                }
                return Ok(administrator);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Administrator>> GetAllAdministrators()
        {
            try
            {
                var administrators = _administratorDAO.GetAllAdministrators();
                return Ok(administrators);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}

