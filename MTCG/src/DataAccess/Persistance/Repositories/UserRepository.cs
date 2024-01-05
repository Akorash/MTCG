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
            UserDTO model = Context.GetUserById(id);
            if (model == null)
            {
                return null;
            }
            return Mapper.Map(model);
        }
        public User GetUserByUsername(string username)
        {
            UserDTO model = Context.GetUserByUsername(username);
            Console.WriteLine("Debug: Successfully got a DTO back");
            if (model == null) 
            {
                return null;
            }
            return Mapper.Map(model);
        }
        public IEnumerable<User> GetAll()
        {
            IEnumerable<UserDTO> dtos = Context.GetAllUsers();
            if (dtos == null)
            {
                return null;
            }
            return dtos.Select(dto => Mapper.Map(dto));
        }

        public void Add(User entity)
        {
            UserDTO dto = Mapper.Map(entity);
            Context.AddUser(dto);
        }
        public void Delete(Guid id)
        {
            if (id != null)
            {
                Context.DeleteUser(id);
            }
        }
    }
}