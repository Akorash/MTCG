using MTCG.src.DataAccess.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain.Entities
{
    public class Card
    {
        public static readonly int MIN_DAMAGE = 5;
        public static readonly int MAX_DAMAGE = 100;
        public static readonly int PACK_PRICE = 5;
        private readonly int DAMAGE_INCREMENTS = 5;
        public enum ElementType
        {
            Fire = 1,
            Water = 2,
            Regular = 3
        }
        public enum CardType
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
        public enum CardNames
        { 
            WaterGoblin = 1,
            FireGoblin = 2, 
            RegularGoblin = 3, 
            WaterTroll = 4, 
            FireTroll = 5, 
            RegularTroll = 6, 
            WaterElf = 7, 
            FireElf = 8, 
            RegularElf = 9, 
            WaterSpell = 10,
            FireSpell = 11, 
            RegularSpell = 12, 
            Knight = 13, 
            Dragon = 14, 
            Ork = 15, 
            Kraken = 16
        }

        public Guid Id { get; private set; }
        public Guid? User { get; private set; } 
        public string Name { get; private set; }
        public string Element { get; private set; }
        public string Type { get; private set; }
        public string? Monster { get; private set; }
        public int Damage { get; private set; }

        public Card(Guid id, Guid? user, string name, string element, string type, string? monster, int damage)
        {
            Id = id;
            User = user; 
            Name = name;
            Element = element;
            if (!type.Equals(CardType.Monster.ToString()) || !type.Equals(CardType.Spell.ToString())) 
            {
                throw new ArgumentException("Could not construct Card: Invalid Type");
            }
            Type = type;
            Monster = monster;
            if (damage <= MIN_DAMAGE || damage > MAX_DAMAGE) 
            {
                throw new ArgumentException("Could not construct Card: Invalid Damage");
            }
            Damage = damage;
        }
        public Card GenerateCard()
        {
            Card card;
            string type = RandomType();
            if (type == CardType.Spell.ToString())
            {
                string element = RandomElement();
                string name = $"{element}{CardType.Spell.ToString()}";
                card = new Card(Guid.NewGuid(), Guid.Empty, name, element, type, null, RandomDamage());
            }
            else
            {
                string element = RandomElement();
                string monster = RandomMonster();
                string name = $"{element}{monster}";
                card = new Card(Guid.NewGuid(), Guid.Empty, name, element, type, monster, RandomDamage());
            }
            return card;
        }
        private string RandomElement()
        {
            var random = new Random();
            var elementTypes = (Card.ElementType[])Enum.GetValues(typeof(ElementType));
            int index = random.Next(0, Enum.GetValues(typeof(ElementType)).Length);
            return elementTypes[index].ToString();
        }
        private string RandomType()
        {
            var random = new Random();
            var cardTypes = (CardType[])Enum.GetValues(typeof(CardType));
            int index = random.Next(0, Enum.GetValues(typeof(CardType)).Length);
            return cardTypes[index].ToString();
        }
        private string RandomMonster()
        {
            var random = new Random();
            var monsterTypes = (MonsterType[])Enum.GetValues(typeof(MonsterType));
            int index = random.Next(0, Enum.GetValues(typeof(MonsterType)).Length);
            return monsterTypes[index].ToString();
        }
        private int RandomDamage()
        {
            var random = new Random();
            int randomIncrement = random.Next(1, MAX_DAMAGE/DAMAGE_INCREMENTS + 1);
            return (MIN_DAMAGE + (randomIncrement * DAMAGE_INCREMENTS));
        }
    }
}
