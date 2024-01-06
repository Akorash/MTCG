using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.src.DataAccess.Core;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;

namespace MTCG.src.DataAccess.Persistance.Mappers
{
    public class UserMapper : IMapper<User, UserDTO>
    {
        public UserMapper() { }
        public User Map(UserDTO userDTO)
        {
            return new User(userDTO.Id, userDTO.BearerToken, userDTO.Username, userDTO.Password, userDTO.Name, userDTO.Bio, userDTO.Image, userDTO.Coins);
        }
        public UserDTO Map(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                BearerToken = user.BearerToken,
                Username = user.Username,
                Password = user.Password,
                Name = user.Name,
                Bio = user.Bio,
                Image = user.Image,
                Coins = user.Coins
            };
        }
    }
}
