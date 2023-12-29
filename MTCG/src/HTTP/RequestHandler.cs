using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.Domain;
using MTCG.src.HTTP;
using MTCG.src.Domain.Entities;
using System.Net;
using System.Net.Sockets;
using System.Data;

namespace MTCG.src.HTTP
{
    public class RequestHandler
    {
        private HttpStatusCode _status;
        public RequestHandler() { }
        // Post Functions ----------
        public void Register(Socket clientSocket, string body)
        {
            // Create User Domain Entity
            var user = new User(null, GetUsername(body), GetPassword(body));
            // Atempt to register User
            try
            {
                user.Register();
                _status = HttpStatusCode.Created;
            }
            catch (DuplicateNameException e) // Username taken
            {
                Console.WriteLine($"Registration failed: {e.Message}");
                _status = HttpStatusCode.Conflict;
            }
            catch (Exception e)
            {
                // TODO fix this error
                Console.WriteLine($"Registration failed: {e.Message}");
                _status = HttpStatusCode.NotFound;
            }
            // Send Response
            var response = new Response(_status);
            response.SendJsonResponse(clientSocket, _status.ToString());
        }
        public void LogIn(Socket clientSocket, string body)
        {
            // Deserialize object
            var user = new User();
            try
            {
                user.LogIn();
            }
            catch (Exception e)
            {

            }
        }
        public string NewPackage(Socket clientSocket, string body)
        {
            return "";
        }
        public string BuyPackage(Socket clientSocket, string body)
        {
            return "";
        }
        public string Battles(Socket clientSocket, string body)
        {
            return "";
        }
        public string Tradings(Socket clientSocket, string body)
        {
            return "";
        }
        public string TradingsWithId(Socket clientSocket, string body)
        {
            return "";
        }
        public string RetrieveUserData(Socket clientSocket, string body)
        {
            return "";
        }
        public string ShowDeck(Socket clientSocket, string body)
        {
            var user = new User();
            return "";
        }
        public string RetrieveStats(Socket clientSocket, string body)
        {
            return "";
        }
        public string RetrieveScoreBoard(Socket clientSocket, string body)
        {
            return "";
        }
        public string ShowCards(Socket clientSocket, string body)
        {
            var user = new User();
            List<Card> cards = user.ShowCards();
            return "";
        }
        static string ShowTrades(Socket clientSocket, string body)
        {
            return "";
        }

        public string GetUsername(string body)
        {
            // TODO: Parse
            return "test1";
        }

        public string GetPassword(string body)
        {
            // TODO: Parse
            return "password1";
        }
    }
}
