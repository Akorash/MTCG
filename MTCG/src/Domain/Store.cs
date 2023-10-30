using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain
{
    internal class Store
    {
        public Store() { }

        private List<Card> _tradeList = new();
        public void ManageTrading(Card toTrade, List<Card> deckOther) { }
        private Card Trade()
        {
            Card _toTrade = new(1, "monster", 30);
            return _toTrade;
        }
        private void AddCard(Card card) { }
        private void RemoveCard(Card card) { }
        private Card RandomCard()
        {
            int _randomId = 1;
            Card _randomCard = new(_randomId, "monster", 30);
            return _randomCard;
        }

        // Get password form db 

        // Inert new Highscore into highscores (sql injection)

        // Insert New User into users (sql injecction)


        // Insert Card for trading (sql injecction)

    }
}
