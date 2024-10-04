using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface IShiftEmployeesDAO
    {
        void AddShiftEmployees(ShiftEmployees shiftEmployees);
        bool UpdateShiftEmployees(ShiftEmployees shiftEmployees);
        ShiftEmployees? GetShiftEmployeesById(int id);
        IEnumerable<ShiftEmployees> GetShiftEmployeess();
    }
}


