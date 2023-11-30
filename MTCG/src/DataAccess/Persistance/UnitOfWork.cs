using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance.Repositories;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess;
using MTCG.src.HTTP;


namespace MTCG.src.DataAccess.Persistance
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private readonly UserMapper _userMapper;
        public UnitOfWork()
        {
            _context = new();
            _userMapper = new();
            Users = new UserRepository(_context, _userMapper);
        }
        public IUserRepository Users { get; private set; } 
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
