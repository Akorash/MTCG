using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Configuration;

using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;


namespace MTCG.src.DataAccess.Persistance
{
    // stringBuider, stringConnectionBuilder
    public class Context : IDisposable
    {
        private readonly string CONNECTION;
        public Context()
        {
            CONNECTION = GetConnectionString();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /*
        public UserDTO GetUserById(int id) 
        {
            try
            {
                using (NpgsqlConnection connection = new(GetConnectionString()))
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NpgsqlException: {ex.Message}");
            }
            return null;
        }
        
        public UserDTO GetUserById(int id)
        {
            using (NpgsqlConnection connection = new(GetConnectionString()))
            {
                Console.WriteLine("Before open\n");
                if (connection.State == ConnectionState.Open)
                {
                    Console.WriteLine("Conn open\n");
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
                        }
                    }
                }
            }
            return null;
        }
        */
        public UserDTO GetUserById(int id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                Console.WriteLine("Before open\n"); // TODO: Remove debug
                connection.Open();

                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return null;
                }
                else if (connection.State == ConnectionState.Open) // ConnectionState can also have other values (ex. Broken, Connecting...)
                {
                    Console.WriteLine("Conn open\n");
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
                        }
                    }
                }
            }
            return null;
        }

        public UserDTO GetUserByUsername(string username)
        {
            using (NpgsqlConnection connection = new(GetConnectionString()))
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
                            var user = new UserDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Username = reader.GetString(reader.GetOrdinal("name")),
                                Password = reader.GetString(reader.GetOrdinal("password"))
                            };
                            Console.WriteLine(user.Id + "\n" + user.Username + "\n" + user.Password + "\n"); // TODO: Remove debug
                            return user;
                        }
                        return null;
                    }
                }
            }
        }
        public List<UserDTO> GetAllUsers()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM users", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var allUsers = new List<UserDTO>();
                            while (reader.Read())
                            {
                                var user = new UserDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Username = reader.GetString(reader.GetOrdinal("name")),
                                    Password = reader.GetString(reader.GetOrdinal("password"))
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
            using (NpgsqlConnection connection = new(GetConnectionString()))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("INSERT INTO users (Name, Email, Password) VALUES (@Name, @Email, @Password)", connection))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(UserDTO user)
        {
            using (NpgsqlConnection connection = new(GetConnectionString()))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new ("DELETE FROM users WHERE Id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(UserDTO user, string[] parameters)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("UPDATE users SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string GetConnectionString()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets("c6fa29a4-4f91-480b-8eae-dcee24e8d186")
                .Build();

                return configuration.GetConnectionString("Postgres");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
