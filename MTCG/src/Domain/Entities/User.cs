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
using MTCG.src.DataAccess.Core;

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
                throw new HttpRequestException("Not Admin."); 
            }

            var package = Card.GeneratePackage();
            if (package == null) 
            { 
                throw new Exception("Failed to generate package."); 
            }

            using (var u = new UnitOfWork())
            {
                foreach (var card in package)
                {
                    Console.WriteLine(card.User);
                    u.Cards.Add(card);
                }
                return package;
            }
        }
        public List<Card> BuyPackage()
        {
            Console.WriteLine(Coins);
            if (!SufficientCoins()) 
            { 
                throw new InvalidOperationException("Insufficient coins."); 
            }

            var package = GetPackage();
            if (package == null) 
            { 
                throw new Exception("Falied to get package."); 
            }

            PayForPackage();

            // If this is the first time buying a package, set it as the user's deck
            using (var u = new UnitOfWork())
            {
                var deck = u.Cards.GetDeck(Id).ToList();
                if (u.Cards.GetDeck(Id).ToList() == null || u.Cards.GetUserCards(Id).ToList() == null) 
                {
                    TransferFromStackToDeck(GetCardIds(package));
                }
            }
            return package;
        }
        public List<Card> ShowCards()
        {
            var cards = GetCardsFromStack();
            cards = cards.Concat(GetCardsFromDeck()).ToList();
            if (cards == null)
            {
                throw new Exception("No cards");
            }
            return cards;
        }
        public List<Card> ShowDeck()
        {
            var cards = new List<Card>();
            using (var unitOfWork = new UnitOfWork())
            {
                cards = unitOfWork.Cards.GetDeck(Id).ToList();
                if (cards == null)
                {
                    throw new Exception("No cards");
                }
                return cards;
            }
        }
        public List<Card> ConfigureDeck(List<Guid> card_ids)
        {
            if (card_ids == null)
            {
                throw new ArgumentNullException();
            }
            TransferFromDeckToStack();
            return TransferFromStackToDeck(card_ids);
        }
        public void RequestTradingDeal(string typeRequirement, int minumumDamage) // Unfinished
        {
            // Must not be in deck
            var cards = GetCardsFromStack();

            // Locked for further usage
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

        private List<Guid> GetCardIds(List<Card> cards)
        {
            var card_ids = new List<Guid>();
            foreach (var card in cards)
            {
                card_ids.Add(card.Id);
            }
            return card_ids;
        }
        private List<Card> GetPackage()
        {
            var package = new List<Card>();
            using (var u = new UnitOfWork())
            {
                package = u.Cards.GetPackage().ToList();
                if (package == null || package.Count < Card.CARDS_IN_PACKAGE)
                {
                    throw new Exception("No card package available for buying");
                }
                foreach (var card in package)
                {
                    Console.WriteLine(card.Id); // Update the owner of the cards
                }
                foreach (var card in package)
                {
                    u.Cards.UpdateUser(card.Id, Id); // Update the owner of the cards
                }
            }
            return package;
        }
        private void PayForPackage()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                Coins -= Card.PACK_PRICE;
                unitOfWork.Users.Update(this);
            }
        }
        private List<Card> GetCardsFromStack()
        {
            var cards = new List<Card>();
            using (var unitOfWork = new UnitOfWork())
            {
                cards = unitOfWork.Cards.GetUserCards(Id).ToList();
                if (cards == null)
                {
                    throw new Exception("No cards");
                }
                return cards;
            }
        }
        private List<Card> GetCardsFromDeck()
        {
            var deck = new List<Card>();
            using (var unitOfWork = new UnitOfWork())
            {
                deck = unitOfWork.Cards.GetDeck(Id).ToList();
                if (deck == null)
                {
                    throw new Exception("No cards");
                }
                return deck;
            }
        }
        public void TransferFromDeckToStack()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var oldDeck = unitOfWork.Cards.GetDeck(Id).ToList();
                if (oldDeck != null)
                {
                    foreach (var card in oldDeck)
                    {
                        // Add the card to the stack
                        unitOfWork.Cards.Add(card);
                        // Remove from the deck
                        unitOfWork.Cards.DeleteFromDeck(card.Id); 
                    }
                }
            }
        }
        public List<Card> TransferFromStackToDeck(List<Guid> card_ids)
        {
            {
                if (card_ids == null)
                {
                    throw new ArgumentNullException();
                }

                var newDeck = new List<Card>();
                using (var unitOfWork = new UnitOfWork())
                {
                    // Transfer chosen cards from stack to deck
                    foreach (var card_id in card_ids)
                    {
                        var card = unitOfWork.Cards.Get(card_id); // Add the card to the deck
                        unitOfWork.Cards.AddToDeck(card);
                        Console.WriteLine("Added to Deck");
                        newDeck.Add(unitOfWork.Cards.Get(card_id)); // Save card in new deck list (to return to the user in the response)
                        Console.WriteLine("Got Card");
                        unitOfWork.Cards.Delete(card_id); // Remove from stack
                        Console.WriteLine("Removed from stack");
                    }
                }

                if (newDeck == null)
                {
                    throw new Exception("Failed to reconfigure deck");
                }
                return newDeck;
            }
        }
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
            return (id == SecretsManager.GetAdminId());
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