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
    public class ShiftAPIClient : IShiftDAO
    {
        private RestClient _client;

        public ShiftAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddShift(Shift shift)
        {
            var request = new RestRequest("api/Shift", Method.Post);
            request.AddJsonBody(shift);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
        }

        public Shift? GetShiftById(int id)
        {
            var request = new RestRequest($"api/Shift/{id}", Method.Get);

            var response = _client.Execute<Shift>(request);
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

        public IEnumerable<Shift> GetShifts()
        {
            var request = new RestRequest("api/Shift", Method.Get);

            var response = _client.Execute<List<Shift>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }

            return response.Data ?? new List<Shift>();
        }

        public bool UpdateShift(Shift shift)
        {
            var request = new RestRequest($"api/Shift/{shift.Id}", Method.Put);
            request.AddJsonBody(shift);

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
