using MTCG.src.DataAccess.Persistance;
using MTCG.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain
{
    public class Round
    {
        private readonly int EFECTIV_MULT = 2;
        private readonly int INEFECTIV_DIV = 1 / 2;
        public bool RoundFinished { get; private set; }
        public Guid? Winner { get; private set; }
        public bool Draw { get; private set; }

        // Initial Damage
        public int InDamagePlayer1 { get; private set; }
        public int InDamagePlayer2 { get; private set; }

        // End Damage
        public int DamagePlayer1 { get; private set; }
        public int DamagePlayer2 { get; private set; }

        // Usernames (for result string)
        public string Player1 { get; private set; }
        public string Player2 { get; private set; }
        public Round()
        {
            RoundFinished = default; // False
            Winner = default;
            Draw = default;

            DamagePlayer1 = default;
            DamagePlayer2 = default;
            InDamagePlayer1 = default;
            InDamagePlayer2 = default;
            Player1 = default;
            Player2 = default;

        }
        public string Play(Card card1, Card card2)
        {
            if (card1 == null || card2 == null) // Check that the cards are valid
            {
                throw new ArgumentException();
            }

            InDamagePlayer1 = card1.Damage;
            InDamagePlayer2 = card2.Damage;

            // Fight
            if (PureMonsterFight(card1, card2))
            {
                Winner = CompareMonsterDamage(card1, card2);
            }
            else if (PureSpellFight(card1, card2))
            {
                Winner = CompareDamage(card1, card2);
            }
            else
            {
                Winner = CompareMixedDamage(card1, card2);
            }

            RoundFinished = true;
            return GenerateResultString(card1, card2);
        }

        // Used for both pure spell fights and mixed fights --> Takes element into account
        private Guid? CompareDamage(Card card1, Card card2)
        {
            // Check and calcualte effectiveness --> Changes damage
            if (WaterVsFire(card1, card2) || FireVsNormal(card1, card2) || NormalVsWater(card1, card2))
            {
                CalculateEffectiveness(card1, card2);
            }
            if (WaterVsFire(card2, card1) || FireVsNormal(card2, card1) || NormalVsWater(card2, card1))
            {
                CalculateEffectiveness(card1, card2);
            }

            // No effect --> No change of damage
            if (DamagePlayer1 == DamagePlayer2)
            {
                Draw = true;
                return null;
            }
            return (DamagePlayer1 > DamagePlayer2) ? card1.Id : card2.Id;
        }
        private Guid? CompareMonsterDamage(Card card1, Card card2)
        {
            // Special cases (See explination underneath)
            if (GoblinVsDragon(card1, card2) || WizzardVsOrk(card1, card2) || FireElfVsDragon(card1, card2))
            {
                return card1.Id;
            }
            if (GoblinVsDragon(card2, card1) || WizzardVsOrk(card2, card1) || FireElfVsDragon(card2, card1))
            {
                return card2.Id;
            }

            // Simply compare damage (element doesn't play a role in pure monster fights)
            if (DamagePlayer1 == DamagePlayer2)
            {
                Draw = true;
                return null;
            }
            return (DamagePlayer1 > DamagePlayer2) ? card1.Id : card2.Id;
        }
        private Guid? CompareMixedDamage(Card card1, Card card2)
        {
            // Special cases (See explination underneath)
            if (KnightVsWaterSpell(card1, card2) || KrakenVsSpell(card1, card2))
            {
                return card1.Id;
            }
            if (KnightVsWaterSpell(card2, card1) || KrakenVsSpell(card2, card1))
            {
                return card2.Id;
            }
            return CompareDamage(card1, card2);
        }
        private string GenerateResultString(Card card1, Card card2)
        {
            var result = $"{Player1}: {card1.Element.ToString()}{card1.Type.ToString()} ({InDamagePlayer1} Damage) " +
                $"vs {Player2}: {card2.Element.ToString()}{card2.Type.ToString()} ({InDamagePlayer1} Damage)" +
                         $"=> {InDamagePlayer1} VS {InDamagePlayer2} -> {DamagePlayer1} vs {DamagePlayer2} => ";
            result += (Draw ? "Draw" : (Winner == card1.Id ? $"{card1.Element.ToString()}{card1.Type.ToString()} wins" : $"{card2.Element.ToString()}{card2.Type.ToString()} wins"));
            return result;
        }

        //---------------------------------------------------------------------
        // ////////////////////////// Effectiveness ///////////////////////////
        //---------------------------------------------------------------------
        private void CalculateEffectiveness(Card card1, Card card2)
        {
            DamagePlayer1 = InDamagePlayer1 * EFECTIV_MULT;
            DamagePlayer2 = InDamagePlayer2 * INEFECTIV_DIV;
        }
        private bool WaterVsFire(Card card1, Card card2)
        {
            return ((card1.Element == Card.ElementType.Water.ToString() && card2.Element == Card.ElementType.Fire.ToString()));
        }
        private bool FireVsNormal(Card card1, Card card2)
        {
            return ((card1.Element == Card.ElementType.Water.ToString() && card2.Element == Card.ElementType.Fire.ToString()));
        }
        private bool NormalVsWater(Card card1, Card card2)
        {
            return ((card1.Element == Card.ElementType.Water.ToString() && card2.Element == Card.ElementType.Fire.ToString()));
        }

        //---------------------------------------------------------------------
        // ////////////////////////// General Cases ///////////////////////////
        //---------------------------------------------------------------------
        private bool PureMonsterFight(Card card1, Card card2)
        {
            return (card1.Type == Card.CardType.Monster.ToString() && card2.Type == Card.CardType.Monster.ToString());
        }
        private bool PureSpellFight(Card card1, Card card2)
        {
            return (card1.Type == Card.CardType.Spell.ToString() && card2.Type == Card.CardType.Spell.ToString());
        }
        private bool SameElement(Card card1, Card card2)
        {
            if ((card1.Element == Card.ElementType.Fire.ToString() && card2.Element == Card.ElementType.Fire.ToString()) ||
                (card2.Element == Card.ElementType.Fire.ToString() && card1.Element == Card.ElementType.Fire.ToString()))
            {
                return true;
            }
            if ((card1.Element == Card.ElementType.Water.ToString() && card2.Element == Card.ElementType.Water.ToString()) ||
               (card2.Element == Card.ElementType.Water.ToString() && card1.Element == Card.ElementType.Water.ToString()))
            {
                return true;
            }
            if ((card1.Element == Card.ElementType.Normal.ToString() && card2.Element == Card.ElementType.Normal.ToString()) ||
               (card2.Element == Card.ElementType.Normal.ToString() && card1.Element == Card.ElementType.Normal.ToString()))
            {
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        // ////////////////////////// Special Cases ///////////////////////////
        //---------------------------------------------------------------------
        // GoblinVsDragon: Goblins are too afraid of Dragons to attack
        // WizzardVsOrk: Wizzard can control Orks so they are not able to damage them
        // FireElfVsDragon: The FireElves know Dragons since they were little and can evade their attacks
        // The armor of Knights is so heavy that WaterSpells make them drown them instantly
        // The Kraken is immune against spells
        private bool GoblinVsDragon(Card card1, Card card2)
        {
            return (card1.Monster == Card.MonsterType.Goblin.ToString() && card2.Monster == Card.MonsterType.Dragon.ToString());
        }
        private bool WizzardVsOrk(Card card1, Card card2)
        {
            return (card1.Monster == Card.MonsterType.Goblin.ToString() && card2.Monster == Card.MonsterType.Dragon.ToString());
        }
        private bool FireElfVsDragon(Card card1, Card card2)
        {
            return ((card1.Monster == Card.MonsterType.Elf.ToString() && card1.Element == Card.ElementType.Fire.ToString()) && card2.Monster == Card.MonsterType.Dragon.ToString());
        }
        private bool KnightVsWaterSpell(Card card1, Card card2)
        {
            return (card1.Monster == Card.MonsterType.Knight.ToString() && card2.Type == Card.CardType.Spell.ToString());
        }
        private bool KrakenVsSpell(Card card1, Card card2)
        {
            return (card1.Monster == Card.MonsterType.Kraken.ToString() && card2.Type == Card.CardType.Spell.ToString());
        }
    }
}
