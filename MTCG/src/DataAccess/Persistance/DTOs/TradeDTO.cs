using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    public class TradeDTO
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public string TypeRequirement { get; set; }
        public int MinimumDamage { get; set; }
    }
}
