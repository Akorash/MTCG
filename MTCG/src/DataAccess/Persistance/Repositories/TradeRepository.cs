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
    internal class TradeRepository : ITradeRepository
    {
        protected readonly PostgreSql Context;
        protected readonly TradeMapper Mapper;
        public TradeRepository(PostgreSql context, TradeMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public Trade Get(Guid id)
        {
            TradeDTO model = Context.GetTradeById(id);
            if (model == null)
            {
                return null;
            }
            return Mapper.Map(model);
        }
        public IEnumerable<Trade> GetAll()
        {
            IEnumerable<TradeDTO> dtos = Context.GetAllTrades();
            if (dtos == null)
            {
                return null;
            }
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(Trade model)
        {

        }
        public void Delete(Guid id)
        {

        }
    }
}
