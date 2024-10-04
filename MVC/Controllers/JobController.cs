using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class JobController : Controller
    {
        IJobDAO _jobDAO;
        ICompanyDAO _companyDAO;

        public JobController(IJobDAO jobDAO, ICompanyDAO companyDAO)
        {
            _jobDAO = jobDAO;
            _companyDAO = companyDAO;
        }

        public IActionResult Index()
        {
            var jobs = _jobDAO.GetAllJobs().Select(job => new
            {
                Job = job,
                CompanyName = _companyDAO.GetCompanyById(job.CompanyID)?.CompanyName
            }).ToList();

            return View(jobs);
        }


        public ActionResult CreateJob()
        {
            var companies = _companyDAO.GetAllCompanys()
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.CompanyName
                }).ToList();

            ViewBag.Company = new SelectList(companies, "Id", "Name");
            return View(new Job());
        }

        [HttpPost]
        public ActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                _jobDAO.AddJob(job);
                return RedirectToAction(nameof(Index));
            }

            var companies = _companyDAO.GetAllCompanys()
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.CompanyName
                }).ToList();

            ViewBag.Company = new SelectList(companies, "Id", "Name");

            return View("CreateJob");
        }

    }
}


