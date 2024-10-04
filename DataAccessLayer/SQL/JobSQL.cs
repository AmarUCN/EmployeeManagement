using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SQL
{
    public class JobSQL : IJobDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public JobSQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddJob(Job job)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "INSERT INTO Job (Place, Department, CompanyID) VALUES (@Place, @Department, @CompanyID)",
                    _connection))
                {
                    command.Parameters.AddWithValue("@Place", job.Place);
                    command.Parameters.AddWithValue("@Department", job.Department);
                    command.Parameters.AddWithValue("@CompanyID", job.CompanyID);

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

        public IEnumerable<Job> GetAllJobs()
        {
            List<Job> jobs = new List<Job>();

            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT JobID, Place, Department, CompanyID FROM Job",
                    _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string place = reader.GetString(1);
                        string department = reader.GetString(2);
                        int companyID = reader.GetInt32(3);

                        jobs.Add(new Job(id, place, department, companyID));
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

            return jobs;
        }

        public Job? GetJobById(int id)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT JobID, Place, Department, CompanyID FROM Job WHERE JobID = @JobID",
                    _connection))
                {
                    command.Parameters.AddWithValue("@JobID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int jobId = reader.GetInt32(0);
                        string place = reader.GetString(1);
                        string department = reader.GetString(2);
                        int companyID = reader.GetInt32(3);

                        return new Job(jobId, place, department, companyID);
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

        public bool UpdateJob(Job job)
        {
            try
            {
                _connection.Open();
                SqlTransaction transaction = _connection.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    using (var updateCommand = new SqlCommand(
                        "UPDATE Job SET Place = @Place, Department = @Department, CompanyID = @CompanyID WHERE JobID = @JobID",
                        _connection,
                        transaction))
                    {
                        updateCommand.Parameters.AddWithValue("@Place", job.Place);
                        updateCommand.Parameters.AddWithValue("@Department", job.Department);
                        updateCommand.Parameters.AddWithValue("@CompanyID", job.CompanyID);
                        updateCommand.Parameters.AddWithValue("@JobID", job.Id);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        transaction.Commit();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Transaction rolled back due to error: {ex.Message}");
                        return false;
                    }
                    catch (Exception exRollback)
                    {
                        Console.WriteLine($"Error during rollback: {exRollback.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
