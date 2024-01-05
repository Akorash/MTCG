﻿using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    internal class CardMapper
    {
        public CardMapper() { }
        public Card Map(CardDTO cardDTO)
        {
            return new Card(cardDTO.Id, cardDTO.Element, cardDTO.Type, cardDTO.Monster, cardDTO.Damage);
        }

        public CardDTO Map(Card card)
        {
            return new CardDTO()
            {
                Id = card.Id,
                Type = card.Type,
                Damage = card.Damage,
                Element = card.Element,
                Monster = card.Monster
            };
        }
    }
}
