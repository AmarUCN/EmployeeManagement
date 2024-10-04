using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface ICompanyDAO
    {
        IEnumerable<Company> GetAllCompanys();
        void AddCompany(Company company);
        bool DeleteCompany(int id);
        Company? GetCompanyById(int Id);
    }
}
