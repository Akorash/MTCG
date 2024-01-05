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
    public class TradeMapper : IMapper<Trade, TradeDTO>
    {
        public TradeMapper() { }
        public Trade Map(TradeDTO tradeDTO)
        {
            return new Trade(tradeDTO.Id, tradeDTO.User, tradeDTO.TypeRequirement, tradeDTO.MinimumDamage);
        }

        public TradeDTO Map(Trade trade)
        {
            return new TradeDTO()
            {
                Id = trade.Id,
                User = trade.User,
                TypeRequirement = trade.TypeRequirement,
                MinimumDamage = trade.MinimumDamage
            };
        }
    }
}
