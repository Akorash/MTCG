using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.HTTP;

namespace TestMTCG
{
    [TestClass]
    public class UnitTestDataHandler
    {
        public UnitTestDataHandler() { }

        [TestMethod]
        private async Task Login()
        {
            RequestHandler dh = new();
            Assert.Equals(dh.Post.Login(""), HttpStatusCode.Accepted);
        }
    }
}
