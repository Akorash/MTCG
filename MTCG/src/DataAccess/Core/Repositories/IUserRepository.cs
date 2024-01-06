using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src;
using MTCG.src.Domain.Entities;

namespace MTCG.src.DataAccess.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Guid GetIdByUsername(string username);
        public User GetByUsername(string username);
        public User GetByToken(string token);
        public void AddToken(BearerToken token);
        public void UpdateUser(Guid card_id, Guid user_id);
    }
}
