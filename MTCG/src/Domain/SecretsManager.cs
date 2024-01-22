using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.Domain
{
    public static class SecretsManager
    {
        public static Guid GetAdminId()
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets("c6fa29a4-4f91-480b-8eae-dcee24e8d186")
                .Build();

                return Guid.Parse(configuration["AdminId"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.Empty;
            }
        }
    }
}
