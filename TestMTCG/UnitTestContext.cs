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
        public void TestContextUser()
        {
            UserDTO user1 = new()
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
    }
}