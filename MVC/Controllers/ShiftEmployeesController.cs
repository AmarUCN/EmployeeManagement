using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ShiftEmployeesController : Controller
{
    private IEmployeeDAO _employeeDAO;
    private IShiftEmployeesDAO _shiftEmployeesDAO;
    private IShiftDAO _shiftDAO; 

    public ShiftEmployeesController(IEmployeeDAO employeeDAO, IShiftEmployeesDAO shiftEmployeesDAO, IShiftDAO shiftDAO)
    {
        _employeeDAO = employeeDAO;
        _shiftEmployeesDAO = shiftEmployeesDAO;
        _shiftDAO = shiftDAO;
    }

    public IActionResult AddEmployeesToShift(int shiftID)
    {
        var shift = _shiftDAO.GetShiftById(shiftID);
        if (shift == null)
        {
            return NotFound(); 
        }

        var shifts = new List<SelectListItem>
    {
        new SelectListItem { Value = shiftID.ToString(), Text = $"{shift.HoursStart} - {shift.HoursEnd} (Salary: {shift.Salary})" }
    };

        var employees = _employeeDAO.GetAllEmployees().Select(e => new
        {
            Id = e.Id,
            FullName = $"{e.FirstName} {e.LastName}"
        }).ToList();

        ViewBag.Shifts = new SelectList(shifts, "Value", "Text", shiftID);
        ViewBag.Employees = employees.Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = e.FullName
        }).ToList();

        return View();
    }

    [HttpPost]
    public IActionResult AddEmployees(int shiftID, int[] employeeIDs, int NumberOfEmployees, string ShiftLocation)
    {
        if (employeeIDs == null || !employeeIDs.Any())
        {
            ModelState.AddModelError("", "Please select at least one employee.");
            return RedirectToAction("AddEmployeesToShift", new { shiftID });
        }

        foreach (var employeeID in employeeIDs)
        {
            _shiftEmployeesDAO.AddShiftEmployees(new ShiftEmployees
            {
                EmployeeID = employeeID,
                ShiftID = shiftID,
                NumberOfEmployees = NumberOfEmployees,  
                ShiftLocation = ShiftLocation           
            });
        }

        return RedirectToAction("Index");
    }



    public IActionResult Index()
    {
        var shiftEmployeesData = _shiftEmployeesDAO.GetShiftEmployeess().Select(shiftEmployee => new
        {
            ShiftEmployee = shiftEmployee,
            ShiftLocation = shiftEmployee.ShiftLocation,
            EmployeeName = _employeeDAO.GetEmployeeById(shiftEmployee.EmployeeID)?.FirstName + " " + _employeeDAO.GetEmployeeById(shiftEmployee.EmployeeID)?.LastName,
            NumberOfEmployees = shiftEmployee.NumberOfEmployees
        }).ToList();

        return View(shiftEmployeesData);
    }
}




