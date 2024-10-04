using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime HoursStart { get; set; }
        public DateTime HoursEnd { get; set; }
        public int Salary { get; set; }
        public int JobID { get; set; }

        public Shift(int id, DateTime hoursStart, DateTime hoursEnd, int salary, int jobID)
        {
            Id = id;
            HoursStart = hoursStart;
            HoursEnd = hoursEnd;
            Salary = salary;
            JobID = jobID;
        }

        public Shift(DateTime hoursStart, DateTime hoursEnd, int salary, int jobID) 
        {
            HoursStart = hoursStart;
            HoursEnd = hoursEnd;
            Salary = salary;
            JobID = jobID;
        }

        public Shift() { }

        public Shift(DateTime hoursStart, DateTime hoursEnd, int salary)
        {
            HoursStart = hoursStart;
            HoursEnd = hoursEnd;
            Salary = salary;
        }

    }
}


