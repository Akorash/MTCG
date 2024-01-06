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
            return new Card(cardDTO.Id, cardDTO.User, cardDTO.Name, cardDTO.Element, cardDTO.Type, cardDTO.Monster, cardDTO.Damage);
        }

        public CardDTO Map(Card card)
        {
            return new CardDTO()
            {
                Id = card.Id,
                User = card.User, 
                Type = card.Type,
                Damage = card.Damage,
                Element = card.Element,
                Monster = card.Monster
            };
        }

        private void ParseName()
        {

        }
    }
}
