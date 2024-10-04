using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccessLayer.Models;
using DataAccessLayer.DAO;

namespace DataAccessLayer.SQL
{
    public class CompanySQL : ICompanyDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public CompanySQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddCompany(Company company)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "INSERT INTO Company (CompanyName, CompanyLocation, AdministratorID) VALUES (@CompanyName, @CompanyLocation, @AdministratorID)",
                    _connection))
                {
                    command.Parameters.AddWithValue("@CompanyName", company.CompanyName);
                    command.Parameters.AddWithValue("@CompanyLocation", company.CompanyLocation);
                    command.Parameters.AddWithValue("@AdministratorID", company.AdministratorID);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public bool DeleteCompany(int id)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "DELETE FROM Company WHERE CompanyID = @CompanyID",
                    _connection))
                {
                    command.Parameters.AddWithValue("@CompanyID", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public IEnumerable<Company> GetAllCompanys()
        {
            List<Company> companies = new List<Company>();

            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT CompanyID, CompanyName, CompanyLocation, AdministratorID FROM Company",
                    _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string companyName = reader.GetString(1);
                        string companyLocation = reader.GetString(2);
                        int administratorID = reader.GetInt32(3);

                        companies.Add(new Company(id, companyName, companyLocation, administratorID));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return companies;
        }

        public Company? GetCompanyById(int id)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT CompanyID, CompanyName, CompanyLocation, AdministratorID FROM Company WHERE CompanyID = @CompanyID",
                    _connection))
                {
                    command.Parameters.AddWithValue("@CompanyID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int companyId = reader.GetInt32(0);
                        string companyName = reader.GetString(1);
                        string companyLocation = reader.GetString(2);
                        int administratorID = reader.GetInt32(3);

                        return new Company(companyId, companyName, companyLocation, administratorID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return null;
        }
    }
}


