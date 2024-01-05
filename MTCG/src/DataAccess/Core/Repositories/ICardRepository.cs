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
        public IEnumerable<Card> GetPackage();
        public void UpdateUser(Guid id, Guid user_id);
    }
}
