using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Place {  get; set; }
        public string Department { get; set; }
        public int CompanyID { get; set; }

        public Job(int id, string place, string department, int companyID)
        {
            Id = id;
            Place = place;
            Department = department;
            CompanyID = companyID;
        }

        public Job(string place, string department, int companyID) 
        {
            Place = place;
            Department = department;
            CompanyID = companyID;
        }

        public Job(string place, string department) 
        {
            Place = place;
            Department = department;
        }

        public Job() { }

    }
}
