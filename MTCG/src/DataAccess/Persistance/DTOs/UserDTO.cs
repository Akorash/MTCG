using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? BearerToken { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Looses { get; set; }
        public int Coins { get; set; }
    }
}
