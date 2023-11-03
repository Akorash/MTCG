using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Core
{
    public interface IMapper<T, TDto> 
    {
        public TDto Map(T entity);
        public T Map(TDto model);
    }
}
