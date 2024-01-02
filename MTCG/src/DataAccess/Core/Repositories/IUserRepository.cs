﻿using System;
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
        public User GetUserByUsername(string username);
    }
}
