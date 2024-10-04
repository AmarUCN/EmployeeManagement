using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public interface IJobDAO
    {
        void AddJob(Job job);
        bool UpdateJob(Job job);
        IEnumerable<Job> GetAllJobs();
        Job? GetJobById(int id);
    }
}
