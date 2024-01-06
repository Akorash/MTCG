using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MTCG.src.Domain.Entities.Card;
using static System.Net.Mime.MediaTypeNames;

namespace MTCG.src.Domain.Entities
{
    public class TradingDeal
    {
        public Guid Id { get; set; }
        public Guid CardToTrade { get; set; }   
        public Guid User { get; set; }
        public string Type { get; set; }
        public int MinimumDamage { get; set; }

        public TradingDeal(Guid id, Guid card, Guid user, string type, int minimumDamage)
        {
            Id = id;
            CardToTrade = card;
            User = user;
            if (!type.Equals(CardType.Monster.ToString()) || !type.Equals(CardType.Spell.ToString()))
            {
                throw new ArgumentException("Could not construct Trade: Invalid Type");
            }
            Type = type;
            if (minimumDamage <= 0 || minimumDamage > Card.MAX_DAMAGE)
            {
                throw new ArgumentException("Could not construct Card: Invalid Damage");
            }
            MinimumDamage = minimumDamage;
        }
    }
}
