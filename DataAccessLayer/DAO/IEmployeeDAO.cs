using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface IEmployeeDAO
    {
        IEnumerable<Employee> GetAllEmployees();
        void AddEmployee(Employee employee);
        bool DeleteEmployee(int id);
        Employee? GetEmployeeById(int Id);
    }
}


