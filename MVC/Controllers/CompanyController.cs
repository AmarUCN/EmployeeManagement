using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class CompanyController : Controller
    {
        ICompanyDAO _companyDAO;
        IAdministratorDAO _administratorDAO;

        public CompanyController(ICompanyDAO companyDAO, IAdministratorDAO administratorDAO)
        {
            _companyDAO = companyDAO;
            _administratorDAO = administratorDAO;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> companies = _companyDAO.GetAllCompanys();
            return View(companies);
        }

        public IActionResult CreateCompany()
        {
            var administrators = _administratorDAO.GetAllAdministrators()
                .Select(a => new
                {
                    Id = a.Id,
                    FullName = $"{a.FirstName} {a.LastName}"
                }).ToList();

            ViewBag.Administrators = new SelectList(administrators, "Id", "FullName");
            return View(new Company());
        }

        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _companyDAO.AddCompany(company);
                return RedirectToAction(nameof(Index));
            }

            var administrators = _administratorDAO.GetAllAdministrators()
                .Select(a => new
                {
                    Id = a.Id,
                    FullName = $"{a.FirstName} {a.LastName}"
                }).ToList();

            ViewBag.Administrators = new SelectList(administrators, "Id", "FullName");

            return View("CreateCompany");
        }

    }
}
