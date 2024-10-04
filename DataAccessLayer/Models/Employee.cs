using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Phone {  get; set; }

        public Employee(int id, string firstName, string lastName, int phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        public Employee(string firstName, string lastName, int phone) 
        { 
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }

        public Employee() { }

    }
}


