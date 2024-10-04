using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SQL
{
    public class EmployeeSQL : IEmployeeDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;
        public EmployeeSQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }
        public void AddEmployee(Employee employee)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "INSERT INTO Employee (FirstName, LastName, Phone) VALUES (@FirstName, @LastName, @Phone)",
                    _connection))
                {
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Phone", employee.Phone);

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


        public bool DeleteEmployee(int id)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "DELETE FROM Employee WHERE EmployeeID = @EmployeeID",
                    _connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", id);

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


        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT EmployeeID, FirstName, LastName, Phone FROM Employee",
                    _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        int phone = reader.GetInt32(3);

                        employees.Add(new Employee(id, firstName, lastName, phone));
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

            return employees;
        }


        public Employee? GetEmployeeById(int id)
        {
            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT EmployeeID, FirstName, LastName, Phone FROM Employee WHERE EmployeeID = @EmployeeID",
                    _connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int employeeId = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        int phone = reader.GetInt32(3);

                        return new Employee(employeeId, firstName, lastName, phone);
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
