using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public ICompanyDAO _companyDAO;

        public CompanyController(ICompanyDAO companyDAO)
        {
            _companyDAO = companyDAO;
        }

        // POST: api/Company
        [HttpPost]
        public ActionResult AddCompany([FromBody] Company company)
        {
            try
            {
                _companyDAO.AddCompany(company);
                return Ok("Company added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // DELETE: api/Company/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCompany(int id)
        {
            try
            {
                bool isDeleted = _companyDAO.DeleteCompany(id);
                if (isDeleted)
                {
                    return Ok("Company deleted successfully.");
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

        // GET: api/Company
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetAllCompanies()
        {
            try
            {
                var companies = _companyDAO.GetAllCompanys();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // GET: api/Company/{id}
        [HttpGet("{id}")]
        public ActionResult<Company> GetCompanyById(int id)
        {
            try
            {
                var company = _companyDAO.GetCompanyById(id);
                if (company == null)
                {
                    return NotFound($"ID {id} not found.");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}

