using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Async, just not written in the name
        // Generic CRUD functions
        public T Get(int id);
        public IEnumerable<T> GetAll();
        public void Add(T t);
        public void Update(T t, string[] parameters);
        public void Delete(T t);
    }
}
