using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ShiftEmployees
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int NumberOfEmployees { get; set; }
        public string ShiftLocation { get; set; }
        public int ShiftID { get; set; }

        public ShiftEmployees(int id, int shiftID, int employeeID, int numberOfEmployees, string shiftLocation)
        {
            Id = id;
            EmployeeID = employeeID;
            ShiftID = shiftID;
            NumberOfEmployees = numberOfEmployees;
            ShiftLocation = shiftLocation;
        }

        public ShiftEmployees(int shiftID, int employeeID, int numberOfEmployees, string shiftLocation) 
        {
            EmployeeID = employeeID;
            ShiftID = shiftID;
            NumberOfEmployees = numberOfEmployees;
            ShiftLocation = shiftLocation;
        }

        public ShiftEmployees() { }

    }
}


