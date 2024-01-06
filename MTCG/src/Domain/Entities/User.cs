using MTCG.src.DataAccess.Persistance;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Drawing;
using System.Security.Authentication;
using System.Xml.Linq;

namespace MTCG.src.Domain.Entities
{
    [Serializable]
    public class User
    {
        public static int START_COINS = 20;

        private int _coins;

        public Guid Id { get; private set; }
        public string BearerToken { get; private set; } 
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

        public User(string bearerToken)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.Users.GetUserByToken(bearerToken);
                    if (user == null)
                    {
                        throw new ArgumentException("Falied to construct user: Token not found");
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

        //------------------------- Authentification --------------------------
        public void Register()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Verification.ValidUsername(Username))
                {
                    throw new ArgumentException();
                }
                // Check whether a user with the same username already exists
                if (unitOfWork.Users.GetIdByUsername(Username) != Guid.Empty)
                {
                    throw new DuplicateNameException(Username);
                }
                // If not, sign the user up
                unitOfWork.Users.Add(this);
            }
        }
        public void LogIn()
        {
            if (!Verification.ValidUsername(Username))
            {
                throw new ArgumentException();
            }
            using (var unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.Users.GetUserByUsername(Username);

                if (user == null) // User doesn't exist
                {
                    throw new InvalidOperationException("User not registered");
                }
                if (!Verification.ValidPassword(Password) || user.Password != Password) // Invlid Password
                {
                    throw new InvalidCredentialException();
                }
            }
        }
        //---------------------------------------------------------------------
        //------------------------------ Cards --------------------------------
        public List<Card> CreatePackage() // Can only be done by admin
        {
            // TOOD Check token for admin
            if (!IsAdmin())
            {
                return null;
            }
            using (var unitOfWork = new UnitOfWork())
            {
                List<Card> cards = (List<Card>)unitOfWork.Cards.GetPackage();
                return cards;
            }
        }
        public List<Card> BuyPackage()
        {
            // TODO UnauthorizedError --> Check Auth Token

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
                return cards;
            }
        }
        public List<Card> ShowDeck()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var deckId = new List<int>();
                var user = unitOfWork.Users.Get(Id);



                // For each cardId in deckIds,
                // List<Card> deck add(GetCardById(cardId)) 

            }
            return new List<Card>();
        }
        public void ConfigureDeck()
        {

        }
        public void RequestTradingDeal()
        {
            // Must not be in deck
            // Locked for further usage
            // Add requirement: spell or monster 
            // Additionaly Type requirement or Minimum Damage
        }
        public List<Tradingdeal> ShowTradingDeals()
        {
            List<Tradingdeal> allTrades;
            using (var unitOfWork = new UnitOfWork())
            {
                allTrades = (List<Tradingdeal>)unitOfWork.Trades.GetAll();
            }
            return allTrades;
        }
        public void Trade(Card card, User other)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.Cards.UpdateUser(card.Id, other.Id);
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
        //---------------------------------------------------------------------
        //------------------------- Other Actions ----------------------------
        public void Battle()
        {
            // If a player is logged in

            // Signal the server (Game) to put you on the waiting list
            // And, send the server your deck (?) and Id
        }
        public void ViewProfile()
        {

        }
        public void ChangeProfile()
        {

        }

        //---------------------------------------------------------------------
        //------------------------- Helper Methods ----------------------------

        private bool SufficientCoins() 
        { 
            return _coins >= Card.PACK_PRICE; 
        }
        private bool CorrectPassword() 
        { 
            return true; 
        }
        private bool IsAdmin()
        {
            return true;
        }
        private string GenerateUUID()
        {
            return "hello";
        }
        private string GetCardIds(List<Card> list, int length)
        {
            if (list.Count <= 0 || list == null)
            {
                return string.Empty;
            }

            string cards = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                cards += list[i].Id.ToString() + "\n";
            }
            return cards;
        }

        static class Verification
        {
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