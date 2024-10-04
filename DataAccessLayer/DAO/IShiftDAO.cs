using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface IShiftDAO
    {
        void AddShift(Shift shift);
        bool UpdateShift(Shift shift);
        Shift? GetShiftById(int id);
        IEnumerable<Shift> GetShifts();
    }
}


