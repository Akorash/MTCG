using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;
using System.Reflection.PortableExecutable;

namespace MTCG.src.DataAccess.Persistance
{
    public class Context : IDisposable
    {
        private readonly string _connection;
        public Context(string connection)
        {
            _connection = connection;
        }

        public UserDTO GetUserById(int id)
        {
            // By wrapping reader in using statements, it, along with the comand and connection get closed and disposed automatically
            // https://stackoverflow.com/questions/744051/is-it-necessary-to-manually-close-and-dispose-of-sqldatareader
            using (NpgsqlConnection connection = new(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("SELECT * FROM users WHERE Id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            // Create a User object and map data from the reader
                            UserDTO user = new UserDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Name")),
                                Password = reader.GetString(reader.GetOrdinal("Password"))
                            };
                            return user;
                        }
                        return null;
                    }
                }
            }
        }
        public UserDTO GetUserByUsername(string username)
        {
            // By wrapping reader in using statements, it, along with the comand and connection get closed and disposed automatically
            // https://stackoverflow.com/questions/744051/is-it-necessary-to-manually-close-and-dispose-of-sqldatareader
            using (NpgsqlConnection connection = new(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("SELECT * FROM users WHERE Username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            // Create a User object and map data from the reader
                            UserDTO user = new UserDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Name")),
                                Password = reader.GetString(reader.GetOrdinal("Password"))
                            };
                            return user;
                        }
                        return null;
                    }
                }
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            using (NpgsqlConnection connection = new(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("SELECT * FROM users", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            List<UserDTO> allUsers = new List<UserDTO>();
                            while (reader.Read())
                            {
                                UserDTO user = new UserDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Username = reader.GetString(reader.GetOrdinal("Name")),
                                    Password = reader.GetString(reader.GetOrdinal("Password"))
                                };
                                allUsers.Add(user);
                            }
                            return allUsers;
                        }
                        return null;
                    }
                }
            }
        }
        public void AddUser(UserDTO user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (Name, Email, Password) VALUES (@Name, @Email, @Password)", connection))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(UserDTO user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM users WHERE Id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(UserDTO user, string[] parameters)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connection))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
