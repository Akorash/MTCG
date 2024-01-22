using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.Domain.Entities;

namespace MTCG.src.DataAccess.Persistance.Repositories
{
    internal class CardRepository : ICardRepository
    {
        protected readonly PostgreSql DbManager;
        protected readonly CardMapper Mapper;
        public CardRepository(PostgreSql manager, CardMapper mapper)
        {
            DbManager = manager;
            Mapper = mapper;
        }

        public Card Get(Guid id)
        {
            var dto = DbManager.GetCardById(id);
            return Mapper.Map(dto);
        }
        public IEnumerable<Card> GetAll()
        {
            IEnumerable<CardDTO> dtos = DbManager.GetAllCards();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public IEnumerable<Card> GetPackage()
        {
            IEnumerable<CardDTO> dtos = DbManager.GetPackage();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public IEnumerable<Card> GetUserCards(Guid user_id)
        {
            IEnumerable<CardDTO> dtos = DbManager.GetUserCards(user_id);
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public IEnumerable<Card> GetDeck(Guid user_id)
        {
            IEnumerable<CardDTO> dtos = DbManager.GetDeck(user_id);
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(Card card)
        {
            CardDTO dto = Mapper.Map(card);
            DbManager.AddCard(dto);
        }
        public void AddToDeck(Card card)
        {
            CardDTO dto = Mapper.Map(card);
            DbManager.AddDeckCard(dto);
        }
        public void Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                throw new ArgumentException("Card id is empty.");
            }
            DbManager.DeleteCard(id);
        }
        public void DeleteFromDeck(Guid id)
        {
            if (id != Guid.Empty)
            {
                throw new ArgumentException("Card id is empty.");
            }
            DbManager.DeleteCard(id);
        }
        public void UpdateUser(Guid id, Guid user_id)
        {
            if (id == Guid.Empty || user_id == Guid.Empty)
            {
                throw new ArgumentException("Card id or User id is empty.");
            }
            DbManager.UpdateUserInCard(id, user_id);
        }
    }
}
