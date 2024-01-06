using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    [Serializable]
    public class CardDTO
    {
        public Guid Id { get; set; }
        public Guid? User { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Monster { get; set; }
        public string Element { get; set; }
        public int Damage { get; set; }

    }
}
