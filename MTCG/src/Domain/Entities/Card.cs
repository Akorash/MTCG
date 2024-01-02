using MTCG.src.DataAccess.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain.Entities
{
    [Serializable]
    public class Card
    {
        private readonly int MAX_ATTK = 30;
        public enum ElementType
        {
            Fire = 1,
            Water = 2,
            Normal = 3
        }
        public static enum CardType
        {
            Monster = 1, 
            Spell = 2
        }
        public enum MonsterType
        {
            Goblin = 1, 
            Knight = 2, 
            Troll = 3, 
            Elf = 4, 
            Wizzard = 5, 
            Ork = 6, 
            Dragon = 7,
            Kraken = 8
        }

        public int Id { get; private set; }
        public string Element { get; private set; }
        public string Type { get; private set; }
        public string? Monster { get; private set; }
        public int Damage { get; private set; }

        public Card(int id, string element, string type, string? monster, int damage)
        {
            Id = id;

            if (!type.Equals(CardType.Monster.ToString()) || !type.Equals(CardType.Spell.ToString())) {
                throw new ArgumentException("Could not construct Card: Invalid Type");
            }
            Type = type;

            if (Type == CardType.Monster.ToString()) {

            }

            if (damage <= 0 || damage > MAX_ATTK) {
                throw new ArgumentException("Could not construct Card: Invalid Damage");
            }
            Damage = damage;
        }                                                                
        public void Create() 
        {
            // Random 

            using (var unitOfWork = new UnitOfWork()) 
            {

            }
            // UnitOfWork --> Save Card in the Database
        }
        public void Trade() 
        {
            // Put int into trade table
        }
        public virtual int Play() 
        { 
            return Damage; 
        }
    }
}
