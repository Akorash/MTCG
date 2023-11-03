using MTCG.src.DataAccess.Core;
using Npgsql.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.DataAccess.Core;

namespace MTCG.src.DataAccess.Persistance.Repositories
{
    /*
    public class Repository<T, TDto> : IRepository<T>
    {
        protected readonly Context Context;

        protected readonly IMapper<T, TDto> Mapper;
        public Repository(Context context, IMapper<T, TDto> mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public T Get(int id)
        {
            // TDto model = Context.Set<TDto>().Find(id);
            TDto model = Context.Find(id);
            return Mapper.Map(model);
        }
        public IEnumerable<T> GetAll()
        {
            IEnumerable<TDto> dtos = Context.ToList();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(T entity)
        {
            TDto dto = Mapper.Map(entity);
            Context.Add(dto);
        }
        public void Delete(T entity)
        {
            TDto dto = Context.Find(entity);
            if (dto != null)
            {
                Context.Remove(dto);
            }
        }
    }
    */
}
