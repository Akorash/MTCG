using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    [Serializable]
    public class BearerTokenDTO
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public string Token { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
