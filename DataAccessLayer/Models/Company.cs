using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public int AdministratorID { get; set; }

        public Company(int id, string companyName, string companyLocation, int administratorID)
        {
            Id = id;
            CompanyName = companyName;
            CompanyLocation = companyLocation;
            AdministratorID = administratorID;
        }

        public Company(string companyName, string companyLocation, int administratorID) 
        {
            CompanyName = companyName;
            CompanyLocation = companyLocation;
            AdministratorID = administratorID;
        }

        public Company(string companyName, string companyLocation) 
        {
            CompanyName = companyName;
            CompanyLocation = companyLocation;
        }
        public Company() { }
    }
}
