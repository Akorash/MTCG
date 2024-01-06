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
            var userId = Guid.NewGuid();
            var user1 = new UserDTO()
            {
                Id = userId,
                Username = "test1",
                Password = "password1"
            };

            UserDTO result = _context.GetUserById(userId);

            Assert.AreEqual(result.Id, user1.Id);
            Assert.AreEqual(result.Username, user1.Username);
            Assert.AreEqual(result.Password, user1.Password);
        }
        [TestMethod]
        public void TestUserGetByUsername()
        {
            var user1 = new UserDTO()
            {
                Id = Guid.NewGuid(),
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
            var cardId = Guid.NewGuid();
            var testCard = new CardDTO()
            {
                Id = cardId,
                Damage = 15,
                Type = "Monster"
            };

            CardDTO result = _context.GetCardById(cardId);
            Assert.AreEqual(result.Id, testCard.Id);
            Assert.AreEqual(result.Damage, testCard.Damage);
            Assert.AreEqual(result.Type, testCard.Type);
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