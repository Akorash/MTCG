using MTCG.src.DataAccess.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain.Entities
{
    [Serializable]
    public class Card
    {
        public enum ElementType
        {
            Fire = 1,
            Water = 2,
            Air = 3,
            Earth = 4,
            Normal = 5
        }
        public int Id { get; private set; }
        public string Type { get; private set; }
        public int Damage { get; }

        public Card(int id, string type, int damage)
        {
            Id = id;
            Type = type;
            Damage = damage;
        }                                                                
        public void Create() 
        {
            // Random 

            using (var unitOfWork = new UnitOfWork()) 
            {

            }
            // UnitOfWork --> Save Card in the Database
        }
        public void Trade() 
        {
            // Put int into trade table
        }
        public virtual int Play() 
        { 
            return Damage; 
        }
    }
}
