using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance.Repositories;
using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess;
using MTCG.src.HTTP;
using MTCG.src.DataAccess.Persistance.Mappers;

namespace MTCG.src.DataAccess.Persistance
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly PostgreSql _manager;
        private readonly UserMapper _userMapper;
        private readonly CardMapper _cardMapper;

        public UnitOfWork()
        {
            _manager = new();
            _userMapper = new();
            _cardMapper = new();

            Users = new UserRepository(_manager, _userMapper);
            Cards = new CardRepository(_manager, _cardMapper);
        }
        public IUserRepository Users { get; private set; } 
        public ICardRepository Cards { get; private set; }
        public void Dispose()
        {
            _manager.Dispose();
        }
    }
}
