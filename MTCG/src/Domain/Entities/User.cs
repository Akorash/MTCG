using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;

namespace MTCG.src.Domain.Entities
{
    public class User
    {
        private readonly int START_COINS = 20;
        private readonly int CARD_PRICE = 5;
        private VerificationHandler _Verif;
        private readonly string _authString;
        private List<Card> _stack;
        private int _coins;
        public int? Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User()
        {
            _Verif = new();
            Username = string.Empty;
            Password = string.Empty;
            _authString = string.Empty;
            _stack = new List<Card>();
            _coins = START_COINS;
        }

        public User(int? id, string uname, string password)
        {
            _Verif = new();
            Id = id;
            Username = uname;
            Password = password;
            _authString = string.Empty;
            _stack = new List<Card>();
            _coins = START_COINS;
        }

        public void SignUp()
        {
            // Check if user already exists
            // Signup
        }

        public string LogIn(string username, string password)
        {
            // Invlid username
            if (!_Verif.ValidUsername(username)) {
                return "401";
            }
            using (var unitOfWork = new UnitOfWork()) 
            {
                User user = unitOfWork.Users.GetUserByUsername(username);
                // User doesn't exist
                if (user == null) {
                    return "4xx";
                }
                // Invlid Password
                if (!_Verif.ValidPassword(password) || user.Password != password) {
                    return "401";
                }
            }
            return "200";
        }
        public List<Card> ShowCards()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                List<Card> cards = (List<Card>)unitOfWork.Cards.GetAll();
                return cards;
            }
        }
        public void ConfigureDeck()
        {

        }
        public List<Card> ShowDeck()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                // List<int> deckIds;
                // unitOfWork.Users.GetDeck()
                // For each cardId in deckIds,
                // List<Card> deck add(GetCardById(cardId)) 

            }
            return new List<Card>();
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
        public void Battle()
        {
            // If a player is logged in
            
            // Signal the server (Game) to put you on the waiting list
            // And, send the server your deck (?) and Id
        }
        private void BuyPackage() { }
        private bool SuffiCoins() { return _coins >= CARD_PRICE; }
        private bool CorrectPassword() { return true; }
    }

    internal class VerificationHandler
    {
        public VerificationHandler() { }
        public bool ValidUsername(string username)
        {
            if (username == null) {
                return false;
            }
            return true;
        }
        public bool ValidPassword(string password)
        {
            if (password == null) {
                return false;
            }
            return true;
        }
        public void UsernameTaken(string username)
        {

        }
        public void IncorrectPassword(string password)
        {

        }
    }
}