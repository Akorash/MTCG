using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.Domain.Entities;

namespace TestMTCG
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestUserLogIn()
        {
            var user = new User(null, "teEEESSttt", "password1", null, null, null, 20);
            string response = user.LogIn();

            Assert.AreEqual(response, null);
        }
    }
}
