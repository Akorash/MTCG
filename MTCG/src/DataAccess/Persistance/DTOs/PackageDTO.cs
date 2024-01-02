using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    public class PackageDTO
    {
        public int? Id { get; set; }
        public int Card1Id { get; set; }
        public int Card2Id { get; set; }
        public int Card3Id { get; set; }
        public int Card4Id { get; set; }
        public int Card5Id { get; set; }
    }
}
