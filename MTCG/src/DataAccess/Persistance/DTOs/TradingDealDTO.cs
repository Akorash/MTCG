using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    [Serializable]
    public class TradingDealDTO
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public Guid Card { get; set; }
        public string Type { get; set; }
        public int MinimumDamage { get; set; }
    }
}
