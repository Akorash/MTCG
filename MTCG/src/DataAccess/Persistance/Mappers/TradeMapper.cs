using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    public class TradeMapper : IMapper<TradingDeal, TradingDealDTO>
    {
        public TradeMapper() { }
        public TradingDeal Map(TradingDealDTO tradeDTO)
        {
            return new TradingDeal(tradeDTO.Id, tradeDTO.Card, tradeDTO.User, tradeDTO.Type, tradeDTO.MinimumDamage);
        }

        public TradingDealDTO Map(TradingDeal trade)
        {
            return new TradingDealDTO()
            {
                Id = trade.Id,
                User = trade.User,
                Type = trade.Type,
                MinimumDamage = trade.MinimumDamage
            };
        }
    }
}
