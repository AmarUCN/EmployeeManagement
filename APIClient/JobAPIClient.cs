using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    public class JobAPIClient : IJobDAO
    {
        private RestClient _client;

        public JobAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }
        public void AddJob(Job job)
        {
            var request = new RestRequest("api/Job", Method.Post);
            request.AddJsonBody(job);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }

        public IEnumerable<Job> GetAllJobs()
        {
            var request = new RestRequest("api/Job", Method.Get);

            var response = _client.Execute<List<Job>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data ?? new List<Job>();
        }

        public Job? GetJobById(int id)
        {
            var request = new RestRequest($"api/Job/{id}", Method.Get);

            var response = _client.Execute<Job>(request);
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data;
        }

        public bool UpdateJob(Job job)
        {
            var request = new RestRequest($"api/Job/{job.Id}", Method.Put);
            request.AddJsonBody(job);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return true;
        }
    }
}