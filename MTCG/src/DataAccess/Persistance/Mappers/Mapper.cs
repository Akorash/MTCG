using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    public class Mapper
    {
        public CardMapper Cards;
        public UserMapper Users;
        public TradeMapper Trades;
        public TokenMapper Tokens;
        public Mapper()
        {
            Cards = new();
            Users = new();
            Trades = new();
            Tokens = new();
        }
    }
}
