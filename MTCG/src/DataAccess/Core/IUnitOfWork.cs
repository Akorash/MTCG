using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Core.Repositories;

namespace MTCG.src.DataAccess.Core
{
    internal interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public ICardRepository Cards { get; }
        public ITradingDealRepository TradingDeals { get; }
    }
}
