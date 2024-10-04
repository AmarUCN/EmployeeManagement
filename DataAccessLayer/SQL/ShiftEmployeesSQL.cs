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
    public class ShiftEmployeesSQL : IShiftEmployeesDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public ShiftEmployeesSQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddShiftEmployees(ShiftEmployees shiftEmployees)
        {
            try
            {
                _connection.Open();
                using (var transaction = _connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // Query to check if the employee is already in a shift with overlapping time
                        using (var checkCommand = new SqlCommand(
                            @"SELECT COUNT(*)
                      FROM ShiftEmployees se
                      JOIN Shift_ s ON se.ShiftID = s.ShiftID
                      WHERE se.EmployeeID = @EmployeeID
                      AND s.HoursEnd_ > (SELECT HoursStart FROM Shift_ WHERE ShiftID = @ShiftID)
                      AND s.HoursStart < (SELECT HoursEnd_ FROM Shift_ WHERE ShiftID = @ShiftID)",
                            _connection, transaction))
                        {
                            checkCommand.Parameters.AddWithValue("@EmployeeID", shiftEmployees.EmployeeID);
                            checkCommand.Parameters.AddWithValue("@ShiftID", shiftEmployees.ShiftID);

                            int overlappingShiftsCount = (int)checkCommand.ExecuteScalar();

                            // If there are any overlapping shifts, throw an exception
                            if (overlappingShiftsCount > 0)
                            {
                                throw new Exception("The employee is already assigned to an overlapping shift.");
                            }
                        }

                        // If no overlap, proceed with the insertion
                        using (var command = new SqlCommand(
                            "INSERT INTO ShiftEmployees (EmployeeID, ShiftID, NumberOfEmployees, ShiftLocation) VALUES (@EmployeeID, @ShiftID, @NumberOfEmployees, @ShiftLocation)",
                            _connection,
                            transaction))
                        {
                            command.Parameters.AddWithValue("@EmployeeID", shiftEmployees.EmployeeID);
                            command.Parameters.AddWithValue("@ShiftID", shiftEmployees.ShiftID);
                            command.Parameters.AddWithValue("@NumberOfEmployees", shiftEmployees.NumberOfEmployees);
                            command.Parameters.AddWithValue("@ShiftLocation", shiftEmployees.ShiftLocation);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception exRollback)
                        {
                            Console.WriteLine($"Error during rollback: {exRollback.Message}");
                            throw;
                        }

                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
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
        }


        public ShiftEmployees? GetShiftEmployeesById(int id)
        {
            try
            {
                _connection.Open();

                string query = "SELECT Id, EmployeeID, ShiftID, NumberOfEmployees, ShiftLocation FROM ShiftEmployees WHERE Id = @Id";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int shiftEmployeesId = reader.GetInt32(0);
                        int employeeID = reader.GetInt32(1);
                        int shiftID = reader.GetInt32(2);
                        int numberOfEmployees = reader.GetInt32(3);
                        string shiftLocation = reader.GetString(4);

                        return new ShiftEmployees
                        {
                            Id = shiftEmployeesId,
                            EmployeeID = employeeID,
                            ShiftID = shiftID,
                            NumberOfEmployees = numberOfEmployees,
                            ShiftLocation = shiftLocation
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return null;
        }

        public IEnumerable<ShiftEmployees> GetShiftEmployeess()
        {
            List<ShiftEmployees> shiftEmployeesList = new List<ShiftEmployees>();

            try
            {
                _connection.Open();

                string query = "SELECT ShiftEmployeesID, EmployeeID, ShiftID, NumberOfEmployees, ShiftLocation FROM ShiftEmployees"; // Ensure the correct ID column is used

                using (var command = new SqlCommand(query, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0); // Make sure this index corresponds to the right column
                        int employeeID = reader.GetInt32(1);
                        int shiftID = reader.GetInt32(2);
                        int numberOfEmployees = reader.GetInt32(3);
                        string shiftLocation = reader.GetString(4);

                        // Log data retrieval
                        Console.WriteLine($"Adding record: ID={id}, EmployeeID={employeeID}, ShiftID={shiftID}, NumberOfEmployees={numberOfEmployees}, ShiftLocation={shiftLocation}");

                        shiftEmployeesList.Add(new ShiftEmployees
                        {
                            Id = id,
                            EmployeeID = employeeID,
                            ShiftID = shiftID,
                            NumberOfEmployees = numberOfEmployees,
                            ShiftLocation = shiftLocation
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return shiftEmployeesList;
        }


        public bool UpdateShiftEmployees(ShiftEmployees shiftEmployees)
        {
            try
            {
                _connection.Open();
                SqlTransaction transaction = _connection.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    using (var command = new SqlCommand(
                        "UPDATE ShiftEmployees SET EmployeeID = @EmployeeID, ShiftID = @ShiftID, NumberOfEmployees = @NumberOfEmployees, ShiftLocation = @ShiftLocation WHERE Id = @Id",
                        _connection,
                        transaction))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", shiftEmployees.EmployeeID);
                        command.Parameters.AddWithValue("@ShiftID", shiftEmployees.ShiftID);
                        command.Parameters.AddWithValue("@NumberOfEmployees", shiftEmployees.NumberOfEmployees);
                        command.Parameters.AddWithValue("@ShiftLocation", shiftEmployees.ShiftLocation);
                        command.Parameters.AddWithValue("@Id", shiftEmployees.Id);

                        int rowsAffected = command.ExecuteNonQuery();
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

