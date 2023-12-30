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
using System.Xml;


namespace MTCG.src.DataAccess.Persistance
{
    public class DBManager : IDisposable
    {
        public DBManager()
        {
            SetSearchPath();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //---------------------------------------------------------------------
        // /////////////////////////////// User ///////////////////////////////
        //---------------------------------------------------------------------
        public UserDTO GetUserById(int id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();

                if (connection != null && connection.State == ConnectionState.Closed) {
                    return null;
                }
                // ConnectionState can also have other values (ex. Broken, Connecting...)
                else if (connection.State == ConnectionState.Open) {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE id = @id;", connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var user = new UserDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Username = reader.GetString(reader.GetOrdinal("username")),
                                    Password = reader.GetString(reader.GetOrdinal("password"))
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
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read()) {
                            // Create a User object and map data from the reader
                            var user = new UserDTO {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("password"))
                            };
                            Console.WriteLine(user.Id + "\n" + user.Username + "\n" + user.Password + "\n"); // TODO: Remove debug
                            return user;
                        }
                    }
                }
            }
            return null;
        }
        public List<UserDTO> GetAllUsers()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM users;", connection))
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
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@Username, @Password)", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(int id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM users WHERE id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateUser(UserDTO user, string[] parameters)
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

        //---------------------------------------------------------------------
        // /////////////////////////////// Card ///////////////////////////////
        //---------------------------------------------------------------------
        public CardDTO GetCardById(int id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return null;
                }
                else if (connection.State == ConnectionState.Open)
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE id = @Id;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var user = new CardDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Type = reader.GetString(reader.GetOrdinal("Type")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("Damage"))
                                };
                                return user;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public List<CardDTO> GetAllCards()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM cards;", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var allCards = new List<CardDTO>();
                            while (reader.Read())
                            {
                                var card = new CardDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("damage"))
                                };
                                allCards.Add(card);
                            }
                            return allCards;
                        }
                        return null;
                    }
                }
            }
        }
        public void AddCard(CardDTO card)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("INSERT INTO cards (type, damage) VALUES (@Type, @Damage)", connection))
                {
                    cmd.Parameters.AddWithValue("@Type", card.Type);
                    cmd.Parameters.AddWithValue("@Damage", card.Damage);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCard(int id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM mtcg.cards WHERE id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //---------------------------------------------------------------------
        // ////////////////////////////// Schema //////////////////////////////
        //---------------------------------------------------------------------
        public void CreateTableUsers()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS  users (" +
                                                        "user_id SERIAL PRIMARY KEY, " +
                                                        "username VARCHAR(50) UNIQUE NOT NULL, " +
                                                        "password VARCHAR(255) NOT NULL" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table users was created successfully.\n");
        }
        public void CreateTableCards()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS  cards (" +
                                                        "card_id SERIAL PRIMARY KEY, " +
                                                        "type VARCHAR(50) NOT NULL, " +
                                                        "damage INT NOT NULL" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table cards was created successfully.\n");
        }
        public void CreateTablePackages()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS packages (" +
                                                        "package_id SERIAL PRIMARY KEY, " +
                                                        "fk_card1 BIGINT, " +
                                                        "fk_card2 BIGINT, " +
                                                        "fk_card3 BIGINT, " +
                                                        "fk_card4 BIGINT, " +
                                                        "fk_card5 BIGINT, " +
                                                        "FOREIGN KEY(fk_card1) REFERENCES cards(card_id), " +
                                                        "FOREIGN KEY(fk_card2) REFERENCES cards(card_id), " +
                                                        "FOREIGN KEY(fk_card3) REFERENCES cards(card_id), " +
                                                        "FOREIGN KEY(fk_card4) REFERENCES cards(card_id), " +
                                                        "FOREIGN KEY(fk_card5) REFERENCES cards(card_id)" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table packages was created successfully.\n");
        }
        public void CreateTableHighscores()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS highscores (" +
                                                        "highscore_id SERIAL PRIMARY KEY," +
                                                        "fk_user BIGINT, " +
                                                        "elo INT NOT NULL, " +
                                                        "wins INT NOT NULL, " +
                                                        "loses INT NOT NULL, " +
                                                        "FOREIGN KEY(fk_user) REFERENCES users(user_id)" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table highscores was created successfully.\n");
        }
        public void CreateTableSessions()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS  sessions (" +
                                                        "session_id SERIAL PRIMARY KEY" +
                                                        "fk_user BIGINT, " +
                                                        "loginTime, " +
                                                        "ipAdress, " +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table sessions was created successfully.\n");
        }
        public void FillTableUsers()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("INSERT INTO users(username, password) VALUES ('test1', 'password1'), ('test2', 'password2'), ('test3', 'password3');", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void CreateSchema()
        {
            try
            {
                DropAll();

                CreateTableUsers();
                CreateTableCards();
                CreateTablePackages();
                CreateTableHighscores();
                // CreateTableSessions();

                FillTableUsers();

            }
            catch (Exception e) 
            {   
                Console.WriteLine(e.Message);
            }
        }
        public void DropAll()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("DROP TABLE IF EXISTS sessions, packages, highscores, cards, users;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //---------------------------------------------------------------------
        private void SetSearchPath()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("SET search_path TO mtcg;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string GetConnectionString()
        {
            try {
                IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets("c6fa29a4-4f91-480b-8eae-dcee24e8d186")
                .Build();

                return configuration.GetConnectionString("Postgres");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
