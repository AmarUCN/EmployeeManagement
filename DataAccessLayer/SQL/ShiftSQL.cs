using DataAccessLayer.DAO;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.SQL
{
    public class ShiftSQL : IShiftDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public ShiftSQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddShift(Shift shift)
        {
            _connection.Open();
            SqlTransaction transaction = _connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                using (var checkCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Shift_ WHERE (@HoursStart < HoursEnd_ AND @HoursEnd_ > HoursStart) AND JobID = @JobID",
                    _connection,
                    transaction))
                {
                    checkCommand.Parameters.AddWithValue("@HoursStart", shift.HoursStart);
                    checkCommand.Parameters.AddWithValue("@HoursEnd_", shift.HoursEnd);
                    checkCommand.Parameters.AddWithValue("@JobID", shift.JobID);

                    int overlappingShifts = (int)checkCommand.ExecuteScalar();

                    if (overlappingShifts > 0)
                    {
                        throw new Exception("Cannot add shift.");
                    }
                }

                using (var insertCommand = new SqlCommand(
                    "INSERT INTO Shift_ (HoursStart, HoursEnd_, Salary, JobID) VALUES (@HoursStart, @HoursEnd_, @Salary, @JobID)",
                    _connection,
                    transaction))
                {
                    insertCommand.Parameters.AddWithValue("@HoursStart", shift.HoursStart);
                    insertCommand.Parameters.AddWithValue("@HoursEnd_", shift.HoursEnd);
                    insertCommand.Parameters.AddWithValue("@Salary", shift.Salary);
                    insertCommand.Parameters.AddWithValue("@JobID", shift.JobID);

                    insertCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                    throw new Exception("Error. The transaction has been rolled back.", ex);
                }
                catch (Exception exRollback)
                {
                    throw new Exception("Error while rolling back the transaction.", exRollback);
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public Shift? GetShiftById(int id)
        {
            try
            {
                _connection.Open();

                string query = "SELECT ShiftID, HoursStart, HoursEnd_, Salary, JobID FROM Shift_ WHERE ShiftID = @ShiftID";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@ShiftID", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int shiftId = reader.GetInt32(0);
                        DateTime hoursStart = reader.GetDateTime(1);
                        DateTime hoursEnd = reader.GetDateTime(2);
                        int salary = reader.GetInt32(3);
                        int jobID = reader.GetInt32(4);

                        return new Shift(shiftId, hoursStart, hoursEnd, salary, jobID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return null;
        }

        public IEnumerable<Shift> GetShifts()
        {
            List<Shift> shifts = new List<Shift>();

            try
            {
                _connection.Open();

                string query = "SELECT ShiftID, HoursStart, HoursEnd_, Salary, JobID FROM Shift_";

                using (var command = new SqlCommand(query, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int shiftId = reader.GetInt32(0);
                        DateTime hoursStart = reader.GetDateTime(1);
                        DateTime hoursEnd = reader.GetDateTime(2);
                        int salary = reader.GetInt32(3);
                        int jobID = reader.GetInt32(4);

                        shifts.Add(new Shift(shiftId, hoursStart, hoursEnd, salary, jobID));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return shifts;
        }

        public bool UpdateShift(Shift shift)
        {
            try
            {
                _connection.Open();
                SqlTransaction transaction = _connection.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    using (var updateCommand = new SqlCommand(
                        "UPDATE Shift_ SET HoursStart = @HoursStart, HoursEnd_ = @HoursEnd, Salary = @Salary, JobID = @JobID WHERE ShiftID = @ShiftID",
                        _connection,
                        transaction))
                    {
                        updateCommand.Parameters.AddWithValue("@HoursStart", shift.HoursStart);
                        updateCommand.Parameters.AddWithValue("@HoursEnd", shift.HoursEnd);
                        updateCommand.Parameters.AddWithValue("@Salary", shift.Salary);
                        updateCommand.Parameters.AddWithValue("@JobID", shift.JobID);
                        updateCommand.Parameters.AddWithValue("@ShiftID", shift.Id);

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


