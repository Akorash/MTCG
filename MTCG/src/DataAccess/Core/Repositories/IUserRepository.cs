using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;

namespace MTCG.src.DataAccess.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        // Add user-specific repository methods here if needed
        public User GetUserByUsername(string username);
    }
}
