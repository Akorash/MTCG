using System.Net.Security;
using MTCG.src.HTTP;

namespace MTCG
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server MTCG = new(8080, 10);
            await MTCG.StartAsync();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}