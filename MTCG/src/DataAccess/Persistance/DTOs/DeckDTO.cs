using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    [Serializable]
    public class DeckDTO
    {
        public Guid Card1Id { get; set; }
        public Guid Card2Id { get; set; }
        public Guid Card3Id { get; set; }
        public Guid Card4Id { get; set; }
    }
}
