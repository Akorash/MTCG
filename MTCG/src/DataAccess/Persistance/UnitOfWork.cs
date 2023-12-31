﻿using System;
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
        private PostgreSql _manager;
        private UserMapper _userMapper;
        private CardMapper _cardMapper;
        private TradeMapper _tradeMapper;
        private TokenMapper _tokenMapper;

        public UnitOfWork()
        {
            _manager = new();
            _userMapper = new();
            _cardMapper = new();
            _tradeMapper = new();
            _tokenMapper = new();

            Users = new UserRepository(_manager, _userMapper, _tokenMapper);
            Cards = new CardRepository(_manager, _cardMapper);
            TradingDeals = new TradingDealRepository(_manager, _tradeMapper);
        }
        public IUserRepository Users { get; private set; } 
        public ICardRepository Cards { get; private set; }
        public ITradingDealRepository TradingDeals { get; private set; }
        public void Dispose()
        {
            _manager.Dispose();
        }
    }
}
