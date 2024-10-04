using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class ShiftController : Controller
    {
        IJobDAO _jobDAO;
        IShiftDAO _shiftDAO;

        public ShiftController(IJobDAO jobDAO, IShiftDAO shiftDAO)
        {
            _jobDAO = jobDAO;
            _shiftDAO = shiftDAO;
        }

        public IActionResult Index()
        {
            var shifts = _shiftDAO.GetShifts().Select(shift => new
            {
                Shift = shift,
                JobPlace = _jobDAO.GetJobById(shift.JobID)?.Place
            }).ToList();

            return View(shifts);
        }


        public ActionResult CreateShift()
        {
            var jobs = _jobDAO.GetAllJobs()
                .Select(j => new
                {
                    Id = j.Id,
                    Name = j.Place 
                }).ToList();

            ViewBag.Job = new SelectList(jobs, "Id", "Name");
            return View(new Shift());
        }

        [HttpPost]
        public ActionResult Create(Shift shift)
        {
            if (ModelState.IsValid)
            {
                _shiftDAO.AddShift(shift);
                return RedirectToAction(nameof(Index));
            }

            var jobs = _jobDAO.GetAllJobs()
                .Select(j => new
                {
                    Id = j.Id,
                    Name = j.Place 
                }).ToList();

            ViewBag.Job = new SelectList(jobs, "Id", "Name");

            return View("CreateShift");
        }

    }
}


