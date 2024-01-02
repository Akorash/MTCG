using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace MTCG.src.Domain
{
    /// <summary>
    /// Your cards are split into 2 categories: 
    /// • monster cards: cards with active attacks and damage based on an element type (fire, water, normal). 
    /// The element type does not effect pure monster fights. 
    /// • spell cards: a spell card can attack with an element based spell (again fire, water, normal) which is: 
    /// – effective (eg: water is effective against fire, so damage is doubled) 
    /// – not effective (eg: fire is not effective against water, so damage is halved) 
    /// – no effect (eg: normal monster vs normal spell, no change of damage, direct comparison between damages) 
    /// Effectiveness: 
    /// • water -> fire 
    /// • fire -> normal 
    /// • normal -> water 
    /// Cards are chosen randomly each round from the deck to compete (this means 1 round is a battle of 2 cards = 1 of each player). 
    /// There is no attacker or defender. All parties are equal in each round. 
    /// Pure monster fights are not affected by the element type. 
    /// As soon as 1 spell cards is played the element type has an effect on the damage calculation of this single round. 
    /// Defeated monsters/spells of the competitor are removed from the competitor’s deck and are taken over in the deck of the current player (vice versa). 
    /// In case of a draw of a round no action takes place (no cards are moved). 
    /// Because endless loops are possible we limit the count of rounds to 100 (ELO stays unchanged in case of a draw of the full game). 
    /// 3 As a result of the battle we want to return a log which describes the battle in great detail. 
    /// Afterwards the player stats (see scoreboard) need to be updated (count of games played and ELO calculation). 
    /// The following specialties are to consider: 
    /// • Goblins are too afraid of Dragons to attack. 
    /// • Wizzard can control Orks so they are not able to damage them. 
    /// • The armor of Knights is so heavy that WaterSpells make them drown them instantly. 
    /// • The Kraken is immune against spells. 
    /// • The FireElves know Dragons since they were little and can evade their attacks.
    /// </summary>
    internal class Game
    {
        private readonly string TITLE = "Monster Trading Card Game";
        private readonly int DECK_LENGTH = 5;
        private readonly int MAX_ROUNDS = 100;
        private User _player1;
        private User _player2;

        public Game() { }
        public void Play(User player1, User player2)
        {
            // Defeated monsters/spells of the competitor are removed from the competitor’s deck and are taken over in the deck of the current player(vice versa). 
            // Cards are chosen randomly each round from the deck to compete (this means 1 round is a battle of 2 cards = 1 of each player).

        }
        private int[] PlayRandom(int[] deck)
        {
            if (deck.Length != 5) {
                return null;
            }
            return ShuffleArray(deck);
        }

        // Fisher-Yates shuffle
        // https://es.wikipedia.org/wiki/Algoritmo_de_Fisher-Yates
        private int[] ShuffleArray(int[] array)
        {
            var random = new Random();

            for (int i = DECK_LENGTH - 1; i > 0; i--) // Progress regressively
            {
                int r = random.Next(i + 1);

                int temp = array[i];
                array[i] = array[r];
                array[r] = temp;
            }
            return array;
        }
    }
}
