using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
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
        private readonly string _authString;
        private List<Card> _stack;
        private int _coins;
        public int? Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
            _authString = string.Empty;
            _stack = new List<Card>();
            _coins = START_COINS;
        }

        public User(int? id, string uname, string password)
        {
            Id = id;
            Username = uname;
            Password = password;
            _authString = string.Empty;
            _stack = new List<Card>();
            _coins = START_COINS;
        }

        public void Register()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Verification.ValidUsername(Username))
                {
                    throw new ArgumentException();
                }
                // Check whether a user with the same username already exists
                if (unitOfWork.Users.GetUserByUsername(Username) != null)
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
        public List<Card> ShowDeck(int UserId)
        {
            using (var unitOfWork = new UnitOfWork())
            {

                var deckId = new List<int>();
                var user = unitOfWork.Users.Get(UserId);



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
        public List<Card> BuyPackage() // Buys a card package with the money of the provided user
        {
            // UnauthorizedError --> Check Auth Token

            // Not enough money for buying a card package
            if (!SuffiCoins())
            {
                throw new InvalidOperationException("Insufficient Coins");
            }
            // No card package available for buying

            // Check the package table 
            // Return an array of cards
            var package = new List<Card>();
            return package;
        }
        private bool SuffiCoins() { return _coins >= CARD_PRICE; }
        static bool CorrectPassword() { return true; }

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
            static void IncorrectPassword(string password)
            {

            }
        }
    }
}