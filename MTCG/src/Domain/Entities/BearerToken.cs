using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain.Entities
{
    public class BearerToken
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public string Token { get; set; }
        public DateTime Timestamp { get; set; }

        public BearerToken(Guid id, Guid user, string token, DateTime timestamp) 
        {
            Id = id;
            User = user;
            Token = token;
            Timestamp = timestamp;
        }

        public BearerToken(Guid user)
        {
            Id = Guid.NewGuid();
            User = user;
            Token = GenerateBearerToken();
            Timestamp = DateTime.Now;
        }
        private string GenerateBearerToken()
        {
            var tokenBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenBytes);
            }
            var token = BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();

            return token;
        }
    }
}
