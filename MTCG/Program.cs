using System.Net.Security;
using MTCG.src.HTTP;
using MTCG.src.DataAccess.Persistance;

namespace MTCG
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cntxt = new DBManager();
            cntxt.GetUserById(1);
            Server MTCG = new(8080, 10);
            await MTCG.StartAsync();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}