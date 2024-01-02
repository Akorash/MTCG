using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Xml;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Configuration;

using MTCG.src.Domain.Entities;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;

namespace MTCG.src.DataAccess.Persistance
{
    public class PostgreSql : IDisposable
    {
        public PostgreSql()
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
                    using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @id;", connection))
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
                                Id = reader.GetInt32(reader.GetOrdinal("user_id")),
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
                using (var cmd = new NpgsqlCommand("DELETE FROM users WHERE user_id = @Id", connection))
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
                using (var cmd = new NpgsqlCommand("UPDATE users SET username = @Username, password = @Password WHERE user_id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
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
                    using (var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE card_id = @Id;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var user = new CardDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("card_id")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("damage"))
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
                                    Id = reader.GetInt32(reader.GetOrdinal("card_id")),
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
        public List<CardDTO> GetPackage() // "SELECT TOP 1 * FROM packages ORDER BY package_id DESC);"
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return null;
                }
                else if (connection.State == ConnectionState.Open)
                {
                    // Get package
                    var packageDTO = new PackageDTO();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM packages ORDER BY package_id DESC LIMIT 1;", connection))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var resultDTO = new PackageDTO
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("package_id")),
                                    Card1Id = reader.GetInt32(reader.GetOrdinal("fk_card1")),
                                    Card2Id = reader.GetInt32(reader.GetOrdinal("fk_card2")),
                                    Card3Id = reader.GetInt32(reader.GetOrdinal("fk_card3")),
                                    Card4Id = reader.GetInt32(reader.GetOrdinal("fk_card4")),
                                    Card5Id = reader.GetInt32(reader.GetOrdinal("fk_card5"))
                                };
                                packageDTO = resultDTO;
                            }
                        }
                    }
                    // Get each card from the package
                    var package = new List<CardDTO>();

                    PropertyInfo[] cardIds = typeof(PackageDTO).GetProperties();
                    foreach (PropertyInfo cardId in packageDTO.GetType().GetProperties().Skip(1))
                    {
                        Console.WriteLine(cardIds.ToString() + cardIds.Length);
                        var value = cardId.GetValue(packageDTO, null); 
                        if (value == null) { 
                            return null; 
                        }
                        int id = Convert.ToInt32(value);
                        package.Add(GetCardById(id));
                    }
                    return package;
                }
            }
            return null;
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
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS users (" +
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
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS cards (" +
                                                        "card_id SERIAL PRIMARY KEY, " +
                                                        "element VARCHAR(50) NOT NULL, " +
                                                        "type VARCHAR(50) NOT NULL, " +
                                                        "monster VARCHAR(50), " +
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
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS sessions (" +
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
                using (var cmd = new NpgsqlCommand("INSERT INTO users(username, password) VALUES " +
                                                        "('test1', 'password1'), " +
                                                        "('test2', 'password2'), " +
                                                        "('test3', 'password3'" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table users filled with test values.\n");
        }
        public void FillTableCards()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("INSERT INTO cards(type, element, monster, damage) VALUES " +
                                                        "(\"Monster\", \"Water\", \"Knight\", 15), " +
                                                        "(\"Spell\", \"Normal\", '',  13), " +
                                                        "(\"Spell\", \"Fire\", '',  20), " +
                                                        "(\"Monster\", \"Water\", \"Dragon\", 12), " +
                                                        "(\"Spell\", \"Water\", '', 17), " +
                                                        "(\"Monster\", \"Fire\", \"Knight\", 13" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table cards filled with test values.\n");
        }
        public void FillTablePackages()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("INSERT INTO packages(fk_card1, fk_card2, fk_card3, fk_card4, fk_card5) VALUES " +
                                                        "(1, 2, 3, 4, 5" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table packages filled with test values.\n");
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
                FillTableCards();
                FillTablePackages();

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
