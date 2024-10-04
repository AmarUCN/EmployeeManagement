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
    public class EmployeeAPIClient : IEmployeeDAO
    {
        private RestClient _client;

        public EmployeeAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddEmployee(Employee employee)
        {
            var request = new RestRequest("api/Employee", Method.Post);
            request.AddJsonBody(employee);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }

        public bool DeleteEmployee(int id)
        {
            var request = new RestRequest($"api/Employee/{id}", Method.Delete);

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

        public IEnumerable<Employee> GetAllEmployees()
        {
            var request = new RestRequest("api/Employee", Method.Get);

            var response = _client.Execute<List<Employee>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data ?? new List<Employee>();
        }

        public Employee? GetEmployeeById(int id)
        {
            var request = new RestRequest($"api/Employee/{id}", Method.Get);

            var response = _client.Execute<Employee>(request);
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
    }
}
