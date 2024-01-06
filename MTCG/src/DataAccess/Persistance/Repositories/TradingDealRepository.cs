using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.Domain.Entities;
using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;

namespace MTCG.src.DataAccess.Persistance.Repositories
{
    internal class TradingDealRepository : ITradingDealRepository
    {
        protected readonly PostgreSql Context;
        protected readonly TradeMapper Mapper;
        public TradingDealRepository(PostgreSql context, TradeMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public TradingDeal Get(Guid id)
        {
            var dto = Context.GetTradingDealById(id);
            return Mapper.Map(dto);
        }
        public IEnumerable<TradingDeal> GetAll()
        {
            IEnumerable<TradingDealDTO> dtos = Context.GetAllTrades();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(TradingDeal dto)
        {
            Context.AddTradingDeal(Mapper.Map(dto));
        }
        public void Delete(Guid id)
        {
            Context.DeleteTradingDeal(id);
        }
    }
}
