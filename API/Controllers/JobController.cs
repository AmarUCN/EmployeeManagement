using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        public IJobDAO _jobDAO;

        public JobController(IJobDAO jobDAO) 
        {
            _jobDAO = jobDAO;
        }

        // POST: api/Job
        [HttpPost]
        public ActionResult AddJob([FromBody] Job job)
        {
            try
            {
                _jobDAO.AddJob(job);
                return Ok("Job added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // PUT: api/Job/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateJob(int id, [FromBody] Job job)
        {
            try
            {
                bool updated = _jobDAO.UpdateJob(job);
                if (updated)
                {
                    return Ok("Job updated successfully.");
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

        // GET: api/Job
        [HttpGet]
        public ActionResult<IEnumerable<Job>> GetAllJobs()
        {
            try
            {
                var jobs = _jobDAO.GetAllJobs();
                return Ok(jobs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Job/{id}
        [HttpGet("{id}")]
        public ActionResult<Job> GetJobById(int id)
        {
            try
            {
                var job = _jobDAO.GetJobById(id);
                if (job == null)
                {
                    return NotFound($"ID {id} not found.");
                }
                return Ok(job);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
