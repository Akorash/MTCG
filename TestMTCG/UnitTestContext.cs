using System.Runtime.CompilerServices;

using MTCG.src.Domain.Entities;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;
using System.ComponentModel.DataAnnotations;

namespace TestMTCG
{
    [TestClass]
    public class UnitTestContext
    {
        private readonly PostgreSql _context;
        private readonly UserMapper _Umapper;
        private readonly TokenMapper _Tokmapper;

        public UnitTestContext()
        {
            _context = new();
            try
            {
                _context.CreateSchema();
            }
            catch (Exception e) 
            {
                throw new Exception($"Failed to construct UnitTestContext: {e.Message}");
            }
            _Umapper = new();
            _Tokmapper = new();
        }

        [TestMethod]
        public void TestGetUserById()
        {
            var user = new UserDTO()
            {
                Username = "hi",
                Password = "hi"
            };
            _context.AddUser(user);

            UserDTO userId = _context.GetUserByUsername(user.Username);

            UserDTO resultUser = _context.GetUserById(userId.Id);

            Assert.AreEqual(resultUser.Username, user.Username);
            Assert.AreEqual(resultUser.Password, user.Password);
        }
        [TestMethod]
        public void TestUserGetByUsername()
        {
            var user = new UserDTO()
            {
                Username = "test1",
                Password = "password1",
                Name = "Juan",
                Bio = "hola guacamola"
            };
            UserDTO result = _context.GetUserByUsername("test1");

            Assert.AreEqual(result.Username, user.Username);
            Assert.AreEqual(result.Password, user.Password);
            Assert.AreEqual(result.Name, user.Name);
            Assert.AreEqual(result.Bio, user.Bio);
            Assert.AreEqual(result.Image, user.Image);
        }
        [TestMethod]
        public void TestToken() 
        {
            UserDTO user = _context.GetUserByUsername("aisa");

            // Check that a token is returned
            var responseToken = _context.GetToken(user.Username);

            // Check that the right user is returned through the token
            UserDTO responseUser = _context.GetUserByToken(responseToken); 

            Assert.IsNotNull(responseToken);
            Assert.AreEqual(responseUser.Username, user.Username);
        }

        [TestMethod]
        public void TestGetPackage()
        {
            var package = new PackageDTO()
            {
                Id = Guid.NewGuid(),
                Card1Id = Guid.NewGuid(),
                Card2Id = Guid.NewGuid(),
                Card3Id = Guid.NewGuid(),
                Card4Id = Guid.NewGuid(),
                Card5Id = Guid.NewGuid()
            };
            var user1 = new UserDTO()
            {
                Id = Guid.NewGuid(),
                Username = "test1",
                Password = "password1"
            };
        }
    }
}