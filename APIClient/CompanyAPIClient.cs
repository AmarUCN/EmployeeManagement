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
    public class CompanyAPIClient : ICompanyDAO
    {
        private RestClient _client;

        public CompanyAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddCompany(Company company)
        {
            var request = new RestRequest("api/Company", Method.Post);
            request.AddJsonBody(company);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }

        public bool DeleteCompany(int id)
        {
            var request = new RestRequest($"api/Company/{id}", Method.Delete);

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

        public IEnumerable<Company> GetAllCompanys()
        {
            var request = new RestRequest("api/Company", Method.Get);

            var response = _client.Execute<List<Company>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data ?? new List<Company>();
        }

        public Company? GetCompanyById(int id)
        {
            var request = new RestRequest($"api/Company/{id}", Method.Get);

            var response = _client.Execute<Company>(request);
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
