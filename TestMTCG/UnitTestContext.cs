using System.Runtime.CompilerServices;

using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Repositories;

namespace TestMTCG
{
    [TestClass]
    public class UnitTestContext
    {
        private readonly PostgreSql _context;

        public UnitTestContext()
        {
            _context = new();
        }

        [TestMethod]
        public void TestUserGet()
        {
            var user1 = new UserDTO()
            {
                Id = 1,
                Username = "test1",
                Password = "password1"
            };

            UserDTO result = _context.GetUserById(1);

            Assert.AreEqual(result.Id, user1.Id);
            Assert.AreEqual(result.Username, user1.Username);
            Assert.AreEqual(result.Password, user1.Password);
        }
        [TestMethod]
        public void TestUserGetByUsername()
        {
            var user1 = new UserDTO()
            {
                Id = 1,
                Username = "test1",
                Password = "password1"
            };

            UserDTO result = _context.GetUserByUsername("test1");

            Assert.AreEqual(result.Id, user1.Id);
            Assert.AreEqual(result.Username, user1.Username);
            Assert.AreEqual(result.Password, user1.Password);
        }
        [TestMethod]
        public void TestCardGet()
        {
            var testCard = new CardDTO()
            {
                Id = 1,
                Damage = 15,
                Type = "Monster"
            };

            CardDTO result = _context.GetCardById(1);
            Assert.AreEqual(result.Id, testCard.Id);
            Assert.AreEqual(result.Damage, testCard.Damage);
            Assert.AreEqual(result.Type, testCard.Type);
        }
        [TestMethod]
        public void TestGetPackage()
        {
            var package = new PackageDTO()
            {
                Id = 1,
                Card1Id = 1,
                Card2Id = 2,
                Card3Id = 3,
                Card4Id = 4,
                Card5Id = 5
            };
            var user1 = new UserDTO()
            {
                Id = 1,
                Username = "test1",
                Password = "password1"
            };
        }
    }
}