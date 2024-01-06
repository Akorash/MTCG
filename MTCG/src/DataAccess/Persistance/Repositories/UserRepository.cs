using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using MTCG.src.DataAccess.Core.Repositories;
using MTCG.src.Domain.Entities;
using MTCG.src.DataAccess.Core;
using System.Reflection;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;

namespace MTCG.src.DataAccess.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly PostgreSql Context;
        protected readonly UserMapper Mapper;
        public UserRepository(PostgreSql context, UserMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public User Get(Guid id)
        {
            UserDTO dto = Context.GetUserById(id);
            return Mapper.Map(dto);
        }
        public IEnumerable<User> GetAll()
        {
            IEnumerable<UserDTO> dtos = Context.GetAllUsers();
            return dtos.Select(dto => Mapper.Map(dto));
        }
        public void Add(User entity)
        {
            var dto = Mapper.Map(entity);
            Context.AddUser(dto);
        }
        public void Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                Context.DeleteUser(id);
            }
        }
        public User GetByUsername(string username)
        {
            var user = Context.GetUserByUsername(username);
            return Mapper.Map(user);
        }
        public User GetByToken(string token)
        {
            var dto = Context.GetUserByToken(token);
            return Mapper.Map(dto);
        }
        public Guid GetIdByUsername(string username)
        {
            return Context.GetIdByUsername(username);
        }
        public void UpdateUser(Guid card_id, Guid user_id) 
        {
            Context.UpdateUserInCard(card_id, user_id);
        }
    }
}