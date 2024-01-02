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
        /*
        [TestMethod]
        private async Task RequestHandlerLogin()
        {
            var rh = new ResponseHandler();
            string body = "body";

            try 
            {
                var server = new Server(8080, 10);
                await server.StartAsync();

                rh.LogIn()
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
        */
    }
}
