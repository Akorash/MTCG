using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    internal class CardDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Damage { get; set; }

    }
}
