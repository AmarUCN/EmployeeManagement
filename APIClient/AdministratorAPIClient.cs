using APIClient.DTO;
using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using RestSharp;
using System.Net;

namespace APIClient;

public class AdministratorAPIClient : IAdministratorDAO
    {
    private RestClient _client;

    public AdministratorAPIClient(string apiResourceUrl)
    {
        _client = new RestClient(apiResourceUrl);
    }

    public int AddAdministrator(Administrator administrator)
    {
        var request = new RestRequest("api/Administrator", Method.Post);
        request.AddJsonBody(administrator);

        var response = _client.Execute<int>(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Error: {response.ErrorMessage}");
        }

        return response.Data;
    }

    public Administrator? GetById(int id)
    {
        var request = new RestRequest($"api/Administrator/{id}", Method.Get);

        var response = _client.Execute<Administrator>(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Error: {response.ErrorMessage}");
        }

        return response.Data;
    }

    public Administrator? Login(string email, string password)
    {
        var request = new RestRequest("api/Administrator/Login", Method.Post);
        var loginRequest = new LoginRequest { Email = email, Password = password };
        request.AddJsonBody(loginRequest);

        var response = _client.Execute<Administrator>(request);
        if (!response.IsSuccessful)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }
            throw new Exception($"Error: {response.ErrorMessage}");
        }

        return response.Data;
    }

    public IEnumerable<Administrator> GetAllAdministrators()
    {
        var request = new RestRequest("api/Administrator", Method.Get);

        var response = _client.Execute<List<Administrator>>(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"Error: {response.ErrorMessage}");
        }

        return response.Data ?? new List<Administrator>();
    }
}