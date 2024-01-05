using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.Domain.Entities;

namespace MTCG.src.Domain
{
    internal class Store
    {
        public Store() { }

        private List<Card> _tradeList = new();
        private void Trade()
        {
    
        }
        private Card RandomCard()
        {
            int _randomId = 1;
            var _randomCard = new Card(_randomId, "Normal", "Monster", "Kraken", 30);
            return _randomCard;
        }
    }
}
