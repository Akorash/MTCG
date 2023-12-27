using System.Runtime.CompilerServices;

using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Repositories;

namespace TestMTCG
{
    [TestClass]
    public class UnitTestContext
    {
        private readonly DBManager _context;

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
    }
}