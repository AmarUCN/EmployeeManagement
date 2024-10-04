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
    public class ShiftEmployeesAPIClient : IShiftEmployeesDAO
    {
        private RestClient _client;

        public ShiftEmployeesAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddShiftEmployees(ShiftEmployees shiftEmployees)
        {
            var request = new RestRequest("api/ShiftEmployees", Method.Post);
            request.AddJsonBody(shiftEmployees);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }

        public ShiftEmployees? GetShiftEmployeesById(int id)
        {
            var request = new RestRequest($"api/ShiftEmployees/{id}", Method.Get);

            var response = _client.Execute<ShiftEmployees>(request);
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

        public IEnumerable<ShiftEmployees> GetShiftEmployeess()
        {
            var request = new RestRequest("api/ShiftEmployees", Method.Get);

            var response = _client.Execute<List<ShiftEmployees>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data ?? new List<ShiftEmployees>();
        }

        public bool UpdateShiftEmployees(ShiftEmployees shiftEmployees)
        {
            var request = new RestRequest($"api/ShiftEmployees/{shiftEmployees.Id}", Method.Put);
            request.AddJsonBody(shiftEmployees);

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