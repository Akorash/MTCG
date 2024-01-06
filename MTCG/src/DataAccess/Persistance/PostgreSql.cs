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
using Newtonsoft.Json.Linq;

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
        public UserDTO GetUserById(Guid id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();

                if (connection != null && connection.State == ConnectionState.Closed) {
                    return null;
                }
                // ConnectionState can also have other values (ex. Broken, Connecting...)
                else if (connection.State == ConnectionState.Open) {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @Id;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var user = new UserDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    BearerToken = reader.GetString(reader.GetOrdinal("name")),
                                    Username = reader.GetString(reader.GetOrdinal("username")),
                                    Password = reader.GetString(reader.GetOrdinal("password")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Bio = reader.GetString(reader.GetOrdinal("bio")),
                                    Image = reader.GetString(reader.GetOrdinal("image")),
                                    Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                                    Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                                    Looses = reader.GetInt32(reader.GetOrdinal("looses")),
                                    Coins = reader.GetInt32(reader.GetOrdinal("coins"))
                                };
                                return user;
                            }
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
                                    Id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    BearerToken = reader.GetString(reader.GetOrdinal("token")),
                                    Username = reader.GetString(reader.GetOrdinal("username")),
                                    Password = reader.GetString(reader.GetOrdinal("password")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Bio = reader.GetString(reader.GetOrdinal("bio")),
                                    Image = reader.GetString(reader.GetOrdinal("image")),
                                    Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                                    Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                                    Looses = reader.GetInt32(reader.GetOrdinal("looses")),
                                    Coins = reader.GetInt32(reader.GetOrdinal("coins"))
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
                using (var cmd = new NpgsqlCommand("INSERT INTO users (user_id, username, password) VALUES (@Id, @Username, @Password)", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteUser(Guid id)
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
                        if (reader != null && reader.Read())
                        {
                            // Create a User object and map data from the reader
                            var user = new UserDTO
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                BearerToken = reader.GetString(reader.GetOrdinal("username")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Bio = reader.GetString(reader.GetOrdinal("bio")),
                                Image = reader.GetString(reader.GetOrdinal("image")),
                                Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                                Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                                Looses = reader.GetInt32(reader.GetOrdinal("looses")),
                                Coins = reader.GetInt32(reader.GetOrdinal("coins"))
                            };
                            return user;
                        }
                    }
                }
            }
            return null;
        }
        public UserDTO GetUserByToken(string token)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT users.user_id, users.username, users.password " +
                                          "FROM tokens " +
                                          "JOIN users ON tokens.fk_user = users.user_id " +
                                          "WHERE tokens.token_id = @Token", connection))
                {
                    cmd.Parameters.AddWithValue("@Tokens", token);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            // Create a User object and map data from the reader
                            var user = new UserDTO
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                BearerToken = token,
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Bio = reader.GetString(reader.GetOrdinal("bio")),
                                Image = reader.GetString(reader.GetOrdinal("image")),
                                Elo = reader.GetInt32(reader.GetOrdinal("elo")),
                                Wins = reader.GetInt32(reader.GetOrdinal("wins")),
                                Looses = reader.GetInt32(reader.GetOrdinal("looses")),
                                Coins = reader.GetInt32(reader.GetOrdinal("coins"))
                            };
                            return user;
                        }
                    }
                }
            }
            return null;
        }
        public Guid GetIdByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            return reader.GetGuid(reader.GetOrdinal("user_id"));
                        }
                    }
                }
            }
            return Guid.Empty;
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
        public CardDTO GetCardById(Guid id)
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
                                var card = new CardDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("card_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("user")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Monster = reader.GetString(reader.GetOrdinal("monster")),
                                    Element = reader.GetString(reader.GetOrdinal("element")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("damage"))
                                };
                                return card;
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var allCards = new List<CardDTO>();
                            while (reader.Read())
                            {
                                var card = new CardDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("card_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("user")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Monster = reader.GetString(reader.GetOrdinal("monster")),
                                    Element = reader.GetString(reader.GetOrdinal("element")),
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
                using (NpgsqlCommand cmd = new("INSERT INTO cards (card_id, fk_user, type, monster, element, damage) VALUES (@Id, @Type, @Monster, @Element, @Damage)", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", card.Id);
                    cmd.Parameters.AddWithValue("@User", card.User);
                    cmd.Parameters.AddWithValue("@Type", card.Type);
                    cmd.Parameters.AddWithValue("@Monster", card.Monster);
                    cmd.Parameters.AddWithValue("@Element", card.Element);
                    cmd.Parameters.AddWithValue("@Damage", card.Damage);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCard(Guid id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM mtcg.cards WHERE card_id = @Id", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<CardDTO> GetPackage()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE user_id IS NULL LIMIT 5;", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var packet = new List<CardDTO>();
                            while (reader.Read())
                            {
                                var card = new CardDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("card_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("user")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Monster = reader.GetString(reader.GetOrdinal("monster")),
                                    Element = reader.GetString(reader.GetOrdinal("element")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("damage"))
                                };
                                packet.Add(card);
                            }
                            return packet;
                        }
                        return null;
                    }
                }
            }
        }
        public List<CardDTO> GetDeck()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM deck_cards;", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var allCards = new List<CardDTO>();
                            while (reader.Read())
                            {
                                var card = new CardDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("card_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("user")),
                                    Type = reader.GetString(reader.GetOrdinal("type")),
                                    Monster = reader.GetString(reader.GetOrdinal("monster")),
                                    Element = reader.GetString(reader.GetOrdinal("element")),
                                    Damage = reader.GetInt32(reader.GetOrdinal("damage")),
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
        public void UpdateUserInCard(Guid card_id, Guid user_id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("UPDATE cards SET fk_user = @UserId WHERE card_id = @CardId", connection))
                {
                    cmd.Parameters.AddWithValue("@CardId", card_id);
                    cmd.Parameters.AddWithValue("@UserId", user_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //---------------------------------------------------------------------
        // ////////////////////////////// Trades //////////////////////////////
        //---------------------------------------------------------------------

        public TradingDealDTO GetTradingDealById(Guid id)
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
                    using (var cmd = new NpgsqlCommand("SELECT * FROM trades WHERE trade_id = @Id;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null && reader.Read())
                            {
                                // Create a User object and map data from the reader
                                var trade = new TradingDealDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("trade_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("fk_user")),
                                    Type = reader.GetString(reader.GetOrdinal("type_requirement")),
                                    MinimumDamage = reader.GetInt32(reader.GetOrdinal("minimum_damage"))
                                };
                                return trade;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public List<TradingDealDTO> GetAllTrades()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM trades;", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            var allTrades = new List<TradingDealDTO>();
                            while (reader.Read())
                            {
                                var trade = new TradingDealDTO
                                {
                                    Id = reader.GetGuid(reader.GetOrdinal("trades_id")),
                                    User = reader.GetGuid(reader.GetOrdinal("fk_user")),
                                    Type = reader.GetString(reader.GetOrdinal("type_requirement")),
                                    MinimumDamage = reader.GetInt32(reader.GetOrdinal("minimum_damage"))
                                };
                                allTrades.Add(trade);
                            }
                            return allTrades;
                        }
                        return null;
                    }
                }
            }
        }
        public void AddTradingDeal(TradingDealDTO trade)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new("INSERT INTO trades (trade_id, fk_user, type_requirement, minimum_damage) VALUES (@Id, @User, @TypeRequirement, @MinimumDamage)", connection))
                {
                    cmd.Parameters.AddWithValue("@Id", trade.Id);
                    cmd.Parameters.AddWithValue("@User", trade.User);
                    cmd.Parameters.AddWithValue("@Type", trade.Type);
                    cmd.Parameters.AddWithValue("@MinimumDamage", trade.MinimumDamage);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTradingDeal(Guid id)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("DELETE FROM trades WHERE trade_id = @Id", connection))
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
                                                        "user_id UUID PRIMARY KEY, " +
                                                        "username VARCHAR(255) UNIQUE NOT NULL, " +
                                                        "password VARCHAR(255) NOT NULL, " +
                                                        "name VARCHAR(255), " +
                                                        "bio TEXT, " +
                                                        "image VARCHAR(255) NOT NULL, " +
                                                        "elo INT DEFAULT 100, " +
                                                        "wins INT DEFAULT 0, " +
                                                        "looses INT DEFAULT 0, " +
                                                        $"coins INT DEFAULT {User.START_COINS} " +
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
                                                        "card_id UUID PRIMARY KEY, " +
                                                        "fk_user UUID REFERENCES users(user_id), " +
                                                        "type VARCHAR(255) NOT NULL CHECK (type IN ('Spell', 'Monster')), " +
                                                        "monster VARCHAR(255) CHECK (type IN ('Kraken', 'Dragon', 'Ork', 'Elf', 'Goblin', 'Knight', 'Wizzard', 'Troll')), " +
                                                        "element VARCHAR(255) NOT NULL CHECK (element IN ('Normal', 'Fire', 'Water')), " +
                                                        "damage DECIMAL NOT NULL" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table cards was created successfully.\n");
        }
        public void CreateTableDeckCards()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS deck_cards (" +
                                                        "deck_card_id UUID PRIMARY KEY, " +
                                                        "fk_user UUID REFERENCES users(user_id), " +
                                                        "type VARCHAR(255) NOT NULL CHECK (type IN ('Spell', 'Monster')), " +
                                                        "monster VARCHAR(255) CHECK (type IN ('Kraken', 'Dragon', 'Ork', 'Elf', 'Goblin', 'Knight', 'Wizzard', 'Troll')), " +
                                                        "element VARCHAR(255) NOT NULL CHECK (element IN ('Normal', 'Fire', 'Water')), " +
                                                        "damage DECIMAL NOT NULL" +
                                                        "position INT CHECK (position >= 1 AND position <= 4), " + // TODO Magic numebrs
                                                        "CONSTRAINT deck_card_id UNIQUE(user_id, card_id)" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table packages was created successfully.\n");
        }
        public void CreateTableTradingDeals()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS trading_deals (" +
                                                        "trading_deal_id UUID PRIMARY KEY, " +
                                                        "fk_user UUID REFERENCES users(user_id), " +
                                                        "fk_card UUID REFERENCES cards(card_id), " +
                                                        "type VARCHAR(50) NOT NULL CHECK (type IN ('Spell', 'Monster')), " +
                                                        $"minimum_damage DECIMAL CHECK (damage >= {Card.MIN_DAMAGE} AND damage <= {Card.MAX_DAMAGE}), " +
                                                        "status VARCHAR(50) NOT NULL CHECK (status IN ('Open', 'Closed'))" +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table trading_deals was created successfully.\n");
        }
        public void CreateTableTokens()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed) {
                    return;
                }
                using (var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS tokens (" +
                                                        "token_id UUID PRIMARY KEY, " +
                                                        "fk_user UUID REFERENCES users(user_id), " +
                                                        "token VARCHAR(255) NOT NULL, " +
                                                        "loginTime TIMESTAMP NOT NULL, " +
                                                    ");", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Database: Table sessions was created successfully.");
        }
        public void CreateSchema()
        {
            try
            {
                DropAll();

                CreateTableUsers();
                CreateTableCards();
                CreateTableDeckCards();
                CreateTableTradingDeals();
                CreateTableTokens();
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
                using (var cmd = new NpgsqlCommand("DROP TABLE IF EXISTS users, cards, deck_cards, trading_deals, tokens;", connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //---------------------------------------------------------------------
        // /////////////////////////// Test Values ////////////////////////////
        //---------------------------------------------------------------------
        
        /*public void FillTableUsers()
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    return;
                }
                using (var cmd = new NpgsqlCommand("INSERT INTO users(user_id, username, password) VALUES " +
                                                        "('', 'password1'), " +
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
        }*/

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
