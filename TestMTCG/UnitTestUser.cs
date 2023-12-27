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
    internal class UnitTestUser
    {
        [TestMethod]
        public void TestSignUpUsernameTaken()
        {
            var user = new User(null, "test1", "password1");
            user.Register();
            // Check response to username taken
            // Check response to user created successfully 
        }
        [TestMethod]
        public void TestUserLogIn()
        {
        }
        [TestMethod]
        public void TestUserShowCards()
        {
        }
        [TestMethod]
        public void TestUserConfigureDeck()
        {
        }
        [TestMethod]
        public void TestUserShowDeck()
        {
        }
    }
}
