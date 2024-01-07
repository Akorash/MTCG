using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    public class TokenMapper
    {
        public TokenMapper() { }
        public BearerToken Map(BearerTokenDTO tokenDTO)
        {
            if (tokenDTO == null)
            {
                return null;
            }
            return new BearerToken(tokenDTO.Id, tokenDTO.User, tokenDTO.Token, tokenDTO.Timestamp);
        }

        public BearerTokenDTO Map(BearerToken token)
        {
            if (token == null)
            {
                return null;
            }
            return new BearerTokenDTO()
            {
                Id = token.Id,
                User = token.User,
                Token = token.Token,
                Timestamp = token.Timestamp
            };
        }
    }
}
