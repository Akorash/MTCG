using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    public class CardMapper : IMapper<Card, CardDTO>
    {
        public CardMapper() { }
        public Card Map(CardDTO cardDTO)
        {
            if (cardDTO == null)
            {
                return null;
            }
            return new Card(cardDTO.Id, cardDTO.User, cardDTO.Name, cardDTO.Element, cardDTO.Type, cardDTO.Monster, cardDTO.Damage);
        }

        public CardDTO Map(Card card)
        {
            if (card == null)
            {
                return null;
            }
            var cardDTO = new CardDTO()
            {
                Id = card.Id,
                User = card.User, 
                Name = card.Name,
                Type = card.Type,
                Damage = card.Damage,
                Element = card.Element,
                Monster = card.Monster
            };
            if (cardDTO.Name == null) // Set Name
            {
                cardDTO.Name = (cardDTO.Type == Card.CardType.Spell.ToString() ? cardDTO.Name = $"{cardDTO.Element}Spell" : cardDTO.Name = $"{cardDTO.Element}{cardDTO.Monster}");
            }
            return cardDTO;
        }
    }
}
