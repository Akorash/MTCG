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
            return new User(userDTO.Id, userDTO.Username, userDTO.Password);
        }

        public UserDTO Map(User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
