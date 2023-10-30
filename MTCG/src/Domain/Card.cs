using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain
{
    [Serializable]
    internal class Card
    {
        private int _id;
        private string _type;
        private int _damage;
        public Card(int id, string type, int damage)
        {
            this._id = id;
            this._type = type;
            this._damage = damage;
        }
        public int Id { get; private set; }
        public enum ElementType
        {
            Fire = 1,
            Water = 2,
            Air = 3,
            Earth = 4,
            Normal = 5
        }
        public int Damage { get; }
        public void CreateCard() { }
        public virtual int Play() { return Damage; }
    }
}
