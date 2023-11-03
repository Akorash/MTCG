using MTCG.src.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Core.Repositories
{
    public interface IRepository<T>
    {
        // Async, just not written in the name
        // Generic CRUD functions
        public T Get(int id);
        public IEnumerable<T> GetAll();

        public void Add(T model);

        public void Delete(T model);
    }
}
