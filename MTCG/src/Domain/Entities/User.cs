using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.Repositories;

namespace MTCG.src.Domain.Entities
{
    public class User
    {
        private readonly int START_COINS = 20;

        private UserVerification _uVerif;

        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        private readonly string _authString;
        private List<Card> _stack;
        private int _coins;

        public User()
        {
            _uVerif = new();

            Username = string.Empty;
            Password = string.Empty;
            _authString = string.Empty;
            _stack = new List<Card>();
            _coins = START_COINS;
        }

        public User(int id, string uname, string password)
        {
            _uVerif = new();

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

        public void LogIn(string username, string password)
        {
            using (UnitOfWork unitOfWork = new())
            {
                User user = unitOfWork.Users.GetUserByUsername("John");
            }
            // check if uname exists
            _uVerif.UserExists(username);

            if (!CorrectPassword())
            {
                // Error
            }

            // hash password
            // compare with that of database
        }

        /*private void ConfigureDeck()
        {
            PrintStack();
            // Get input from user
            // ChooseCard() five times
            // While i > 5 
            // Deck[i] = 

            // OPT: Save cards in a list
        }*/

        // TODO: Change this
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

        // Server request, provides deck

        // Request to Log in 

        // Make sure you can only call the Battle method when logged in
        private void Battle()
        {
            // If a player is logged in
            // If a player requests the server to play
            // Signal the server (Game) to put you on the waiting list
            // And, send the server your deck (?) and Id
        }
        private int ChooseCard()
        {
            // Gets input from user (number)
            // Only indecies from the deck
            // If input is valid, returns index from deck
            return 0;
        }
        private void BuyPackage() { }
        private bool SuffiCoins() { return _coins >= 5; }
        private bool CorrectPassword() { return true; }
    }

    internal class UserVerification
    {
        public UserVerification() { }
        public void UserExists(string username)
        {

        }
        public void UsernameTaken(string username)
        {

        }
        public void IncorrectPassword(string password)
        {

        }
    }
}