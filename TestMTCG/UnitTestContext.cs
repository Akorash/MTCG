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
            _Umapper = new();
            _Tokmapper = new();
        }

        [TestMethod]
        public void TestGetUserById()
        {
            var userId = Guid.NewGuid();
            var user = new UserDTO()
            {
                Id = userId,
                Username = "test4",
                Password = "password4"
            };
            _context.AddUser(user);

            UserDTO result = _context.GetUserById(userId);

            Assert.AreEqual(result.Id, user.Id);
            Assert.AreEqual(result.Username, user.Username);
            Assert.AreEqual(result.Password, user.Password);
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
            var user = _context.GetUserByUsername("aisa");
            var response = _context.GetToken(user.Username);

            var responseUser = _context.GetUserByToken(response);

            Assert.AreNotEqual(user.Id, Guid.Empty);
            Assert.IsNotNull(response);
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