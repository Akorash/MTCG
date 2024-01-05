using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MTCG.src.Domain.Entities.Card;
using static System.Net.Mime.MediaTypeNames;

namespace MTCG.src.Domain.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public string TypeRequirement { get; set; }
        public int MinimumDamage { get; set; }

        public Trade(Guid id, Guid user, string typeRequirement, int minimumDamage)
        {
            Id = id;
            User = user;

            if (!typeRequirement.Equals(CardType.Monster.ToString()) || !typeRequirement.Equals(CardType.Spell.ToString()))
            {
                throw new ArgumentException("Could not construct Trade: Invalid Type");
            }
            TypeRequirement = typeRequirement;
            if (minimumDamage <= 0 || minimumDamage > Card.MAX_DAMAGE)
            {
                throw new ArgumentException("Could not construct Card: Invalid Damage");
            }
            MinimumDamage = minimumDamage;
        }
    }
}
