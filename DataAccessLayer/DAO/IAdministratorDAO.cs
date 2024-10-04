using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface IAdministratorDAO
    {
        Administrator? Login(string email, string password);
        Administrator? GetById (int id);
        int AddAdministrator(Administrator administrator);
        IEnumerable<Administrator> GetAllAdministrators();
    }
}
