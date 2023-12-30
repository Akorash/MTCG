using System;
using System.Collections.Generic;
using System.Data;
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

        // https://stackoverflow.com/questions/741029/best-way-to-test-exceptions-with-assert-to-ensure-they-will-be-thrown

        [TestMethod]
        private void RequestHandlerLogin()
        {
            var rh = new ResponseHandler();
            string body = "body";

            try 
            {
                rh.LogIn(body);
                Assert.Fail("An exception should have been thrown");
            }
            catch (DuplicateNameException e) 
            {
                Assert.AreEqual("", e.Message);
            }
            catch (Exception e)
            {
                Assert.Fail($"Unexpected exception of type {0} caugth: {1}", e.GetType(), e.Message);
            }
        }
    }
}
