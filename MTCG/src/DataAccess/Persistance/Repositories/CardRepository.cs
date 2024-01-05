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
            CardDTO model = DbManager.GetCardById(id);
            return Mapper.Map(model);
        }
        public IEnumerable<Card> GetAll()
        {
            IEnumerable<CardDTO> dtos = DbManager.GetAllCards();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public IEnumerable<Card> GetPackage()
        {
            IEnumerable<CardDTO> dtos = DbManager.GetPackage();
            Console.WriteLine();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(Card entity)
        {
            CardDTO dto = Mapper.Map(entity);
            DbManager.AddCard(dto);
        }
        public void UpdateUser(Guid id, Guid user_id)
        {
            DbManager.UpdateUserInCard(id, user_id);
        }
        public void Delete(Guid id)
        {
            if (id != null) {
                DbManager.DeleteCard(id);
            }
        }
    }
}
