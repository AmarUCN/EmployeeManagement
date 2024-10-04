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
    public class AdministratorSQL : IAdministratorDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;
        public AdministratorSQL(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }
        public int AddAdministrator(Administrator administrator)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO Administrator (FirstName, LastName, Email, PasswordWithHash) VALUES (@FirstName, @LastName, @Email, @PasswordWithHash); SELECT SCOPE_IDENTITY();",
                    connection);

                command.Parameters.AddWithValue("@FirstName", administrator.FirstName);
                command.Parameters.AddWithValue("@LastName", administrator.LastName);
                command.Parameters.AddWithValue("@Email", administrator.Email);
                command.Parameters.AddWithValue("@PasswordWithHash", administrator.Password);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }


        public Administrator? GetById(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Administrator WHERE AdministratorID = @AdministratorID", connection);
                command.Parameters.AddWithValue("@AdministratorID", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Administrator(
                            Convert.ToInt32(reader["AdministratorID"]),
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            reader["Email"].ToString(),
                            reader["PasswordWithHash"].ToString()
                        );
                    }
                }
            }
            return null;
        }


        public Administrator? Login(string email, string password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Administrator WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedPassword = reader["PasswordWithHash"].ToString();
                        if (storedPassword == password)
                        {
                            return new Administrator(
                                Convert.ToInt32(reader["AdministratorID"]),
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                email,
                                storedPassword
                            );
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Administrator> GetAllAdministrators()
        {
            List<Administrator> administrators = new List<Administrator>();

            try
            {
                _connection.Open();

                using (var command = new SqlCommand(
                    "SELECT AdministratorID, FirstName, LastName, Email, PasswordWithHash FROM Administrator",
                    _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string email = reader.GetString(3);
                        string password = reader.GetString(4);

                        administrators.Add(new Administrator(id,firstName,lastName,email,password));
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

            return administrators;
        }
    }
}
