using MTCG.src.DataAccess.Persistance;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using MTCG.src.DataAccess.Persistance.DTOs;

namespace MTCG.src.Domain.Entities
{
    public class User
    {
        public static int START_COINS = 20;

        private int _coins;

        public Guid Id { get; private set; }
        public string? BearerToken { get; private set; } 
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string? Name { get; private set; }
        public string? Bio { get; private set; }
        public string? Image { get; private set; }
        public int Elo { get; private set; }
        public int Wins { get; private set; }
        public int Looses { get; private set; }
        public int Coins { get; private set; }
        
        public List<Card> Deck { get; private set; }

        public User(string bearerToken) // Checks if the token is valid
        {
            if (bearerToken == null || bearerToken == string.Empty )
            {
                throw new ArgumentNullException(nameof(bearerToken));
            }
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    // TODO: Check if expired --> Add _context.GetToken to user repository 
                    var user = unitOfWork.Users.GetByToken(bearerToken);
                    if (user == null)
                    {
                        throw new ArgumentException("Invalid Token");
                    }
                    Id = user.Id;
                    BearerToken = user.BearerToken;
                    Username = user.Username;
                    Password = user.Password;
                    Name = user.Name;
                    Bio = user.Bio;
                    Image = user.Image;
                    Coins = user.Coins;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public User(Guid id, string token, string username, string password, string name, string bio, string image, int coins)
        {
            Id = id;
            BearerToken = token;
            Username = username;
            Password = password;
            Name = name;
            Bio = password;
            Image = password;
            Coins = coins;
        }
        public User(string token, string username, string password, string name, string bio, string image, int coins)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    Id = unitOfWork.Users.GetIdByUsername(username);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            BearerToken = token;
            Username = username;
            Password = password;
            Name = name;
            Bio = password;
            Image = password;
            Coins = coins;
        }

        //------------------------- Authentification --------------------------
        public void Register()
        {
            Id = Guid.NewGuid();
            using (var unitOfWork = new UnitOfWork())
            {
                // Check if username is taken
                if (unitOfWork.Users.GetIdByUsername(Username) != Guid.Empty)
                {
                    throw new DuplicateNameException(Username);
                }
                // If not, sign the user up
                Console.WriteLine("User id: " + Id);
                unitOfWork.Users.Add(this);
            }
        }
        public string LogIn()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.Users.GetByUsername(Username);
                if (user == null) // User doesn't exist
                {
                    throw new InvalidOperationException("User not registered");
                }
                if (Password != user.Password) // User doesn't exist
                {
                    throw new InvalidCredentialException("Passwords don't match");
                }
                // Create and store token in database
                Id = unitOfWork.Users.GetIdByUsername(Username);
                var bearerToken = new BearerToken(Id);
                unitOfWork.Users.AddToken(bearerToken);
                return bearerToken.Token;
            }
        }
        //---------------------------------------------------------------------
        //------------------------------ Cards --------------------------------
        public List<Card> CreatePackage(Guid id)
        {
            if (!IsAdmin(id))
            {
                throw new HttpRequestException("Not Admin");
            }
            var package = Card.GeneratePackage();
            if (package == null)
            {
                throw new Exception("Failed to generate package");
            }
            using (var unitOfWork = new UnitOfWork())
            {
                foreach (var card in package)
                {
                    unitOfWork.Cards.Add(card);
                }
                return package;
            }
        }
        public List<Card> BuyPackage()
        {
            Console.WriteLine(Coins);
            if (!SufficientCoins())
            {
                throw new InvalidOperationException("Insufficient Coins");
            }
            var package = new List<Card>();
            using (var unitOfWork = new UnitOfWork())
            {
                package = unitOfWork.Cards.GetPackage().ToList(); // GetPackage() returns IEnumerable<Card>, hence the .ToList()
                if (package == null)
                {
                    throw new Exception("No card package available for buying");
                }
            }
            return package;
        }
        public List<Card> ShowCards()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var cards = (List<Card>)unitOfWork.Cards.GetAll();
                if (cards == null)
                {
                    throw new Exception("No cards");
                }
                return cards;
            }
        }
        public List<Card> ShowDeck()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var cards = (List<Card>)unitOfWork.Cards.GetDeck(Id);
                if (cards == null)
                {
                    throw new Exception("No cards");
                }
                return cards;
            }
        }
        public List<Card> ConfigureDeck(List<Guid> card_ids) // Unfinished
        {
            if (card_ids == null)
            {
                throw new ArgumentNullException();
            }
            using (var unitOfWork = new UnitOfWork())
            {
                // unitOfWork.Users.UpdateDeck // TODO: Create UpdateDeck statement and add to UserRepository
                // var result = unitOfWork.Users.GetDeck(Id);
                // if (result == null)
                // {
                //     throw new Exception("Failed to reconfigure deck");
                // }
                // return result;
            }
            return new List<Card>();
        }
        public void RequestTradingDeal() // Unfinished
        {
            // Must not be in deck
            // Locked for further usage
            // Add requirement: spell or monster 
            // Additionaly Type requirement or Minimum Damage
        }
        public List<TradingDeal> ShowTradingDeals()
        {
            List<TradingDeal> allTrades;
            using (var unitOfWork = new UnitOfWork())
            {
                allTrades = (List<TradingDeal>)unitOfWork.TradingDeals.GetAll();
            }
            if (allTrades == null)
            {
                throw new Exception("No trading deals available");
            }
            return allTrades;
        }
        public void Trade(Card card, User other)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.Cards.UpdateUser(card.Id, other.Id); // Change the user_id to the new owner
            }
        }

        //---------------------------------------------------------------------
        //------------------------- Other Actions ----------------------------
        public void Battle() // Unfinished
        {
            // TODO: Signal the server (Game) to put you in the battle queue
            
            // TODO: Wait for the server to send you the response
        }
        public List<string> ViewProfile()
        {
            return new List<string>() { Username, Name, Bio, Image };
        }
        public User ChangeProfile(string name, string bio, string image) // Unfinished
        {
            using (var unitOfWork = new UnitOfWork())
            {
                // unitOfWork.Users.UpdateUser(Username, name, bio, image);
                // var user = unitOfWork.Users.GetByUsername(Username);
                // if (user.Name != name || user.Bio != bio || user.Image != image)
                //{
                //     throw new Exception($"Could not change {Username}'s profile");
                //}
                // return user;
            }
            return this;
        }
        public List<string> ViewStats() // Unfinished
        {
            using (var unitOfWork = new UnitOfWork())
            {
                // TODO: Add GetStats to UserRepository, retrieve elo, wins and looses
            }
            // Temporary return value
            var stats = new List<string>();
            return stats;
        }
        public List<User> ViewScoreBoard() // Unfinished
        {
            using (var unitOfWork = new UnitOfWork())
            {
                // TODO: Create GetScoreboard function and add it to UserRepository
            }
            // Temporary return value
            var scoreboard = new List<User>();
            return scoreboard;
        }
        public User ViewUserData(string usernameOther)
        {
            if (!IsAdmin(Id))
            {
                throw new HttpRequestException("Not Admin");
            }
            using (var unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.Users.GetByUsername(usernameOther);
                if (user == null)
                {
                    throw new ArgumentException("Invalid username");
                }
                return user;
            }
        }
        public User UpdateUserData(User other) // Unfinished
        {
            if (other == null)
            {
                throw new ArgumentException("No data corresponding the user to be updated was provided");
            }
            if (!IsAdmin(Id))
            {
                throw new HttpRequestException("Not Admin");
            }
            using (var unitOfWork = new UnitOfWork())
            {
                // unitOfWork.Users.UpdateUser(other);
                // var result = unitOfWork.Users.GetByUsername(other.Username);
                // if (result == null)
                // {
                //     throw new Exception("Failed to update user");
                // }
                // return result;
            }
            return this;
        }

        //---------------------------------------------------------------------
        //------------------------- Helper Methods ----------------------------
        public void AddToDeck(Card card)
        {
            Deck.Add(card);
        }
        public void RemoveFromDeck(Card card)
        {
            int index = Deck.IndexOf(card);
            Deck.RemoveAt(index);
        }
        public void SetDeck(List<Card> deck)
        {
            this.Deck = deck;
        }
        private bool SufficientCoins() 
        { 
            return Coins >= Card.PACK_PRICE; 
        }
        private bool IsAdmin(Guid id)
        {
            return (id == GetAdminId());
        }
        private Guid GetAdminId()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets("c6fa29a4-4f91-480b-8eae-dcee24e8d186")
                .Build();

                return Guid.Parse(configuration["AdminId"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.Empty;
            }
        }

        //---------------------------------------------------------------------
        static class Verification
        {
            /* if (!Verification.ValidUsername(Username))
            {
                throw new ArgumentException();
            }*/
            /*if (!Verification.ValidPassword(Password) || user.Password != Password) // Invlid Password
            {
                throw new InvalidCredentialException();
            }*/
            public static bool ValidUsername(string username)
            {
                if (username == null)
                {
                    return false;
                }
                return true;
            }
            public static bool ValidPassword(string password)
            {
                if (password == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}