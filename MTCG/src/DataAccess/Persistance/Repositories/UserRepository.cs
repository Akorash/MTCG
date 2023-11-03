using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.Domain.Entities;
using MTCG.src.DataAccess.Core;

namespace MTCG.src.DataAccess.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly Context Context;
        protected readonly UserMapper Mapper;
        public UserRepository(Context context, UserMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public User Get(int id)
        {
            UserDTO model = Context.GetUserById(id);
            return Mapper.Map(model);
        }
        public User GetUserByUsername(string username)
        {
            UserDTO model = Context.GetUserByUsername(username);
            return Mapper.Map(model);
        }
        public IEnumerable<User> GetAll()
        {
            IEnumerable<UserDTO> dtos = Context.GetAllUsers();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(User entity)
        {
            UserDTO dto = Mapper.Map(entity);
            Context.AddUser(dto);
        }
        public void Delete(User entity)
        {
            UserDTO dto = Context.GetUserById(entity.Id);
            if (dto != null)
            {
                Context.DeleteUser(dto);
            }
        }
    }
}