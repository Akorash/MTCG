using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain
{
    internal class Game
    {
        private const string TITLE = "Monster Trading Card Game";

        // In DB:

        private readonly List<User>? _users;
        private readonly List<Card>? _cards;
        private readonly List<Card>? _highscores;


        private List<User> _waitingList = new();

        // Instances of Battles once two players are matched
        public Game() { }

        public Game(List<User> users, List<Card> cards, List<Card> highscores)
        {
            this._users = users;
            this._cards = cards;
            this._highscores = highscores;
        }
        public void Run()
        {
            // If server recieves a play from the user
        }

        // Get from DB
        private void ShowScoreboard() { }
        void Match() { }
    }
}
