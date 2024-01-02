using System.Net.Security;
using MTCG.src.HTTP;
using MTCG.src.DataAccess.Persistance;

namespace MTCG
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Database
            var cntxt = new PostgreSql();
            cntxt.CreateSchema();

            // Web Api (Server)
            var MTCG = new Server(8080, 10);
            await MTCG.StartAsync();

            // Exit
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}