using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.DataAccess.Persistance;
using MTCG.src;
using MTCG.src.Domain.Entities;


namespace MTCG.src.DataAccess.Core.Repositories
{
    internal interface ICardRepository : IRepository<Card>
    {
        public void AddToDeck(Card card);
        public IEnumerable<Card> GetPackage();
        public IEnumerable<Card> GetUserCards(Guid user_id);
        public IEnumerable<Card> GetDeck(Guid user_id);
        public void UpdateUser(Guid id, Guid user_id);
        public void DeleteFromDeck(Guid id);
    }
}
