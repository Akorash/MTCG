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
using System.ComponentModel;
using System.Security.Authentication;
using System.Reflection;
using Newtonsoft.Json;

namespace MTCG.src.HTTP
{
    /// <summary>
    /// The <c>ResponseHandler<\c> class is essentially responsable for assigning the correct Http Status-Code
    /// depending whether the Domain Level Method being called throws an exception, and which.
    /// It also sends the HTTP response and shuts down the client socket.
    /// 
    /// It is not responsable for parsing the request or selecting which function should be called for 
    /// a given URL, that is done in within the <c>Server</c> class.
    /// </summary>
    public class ResponseHandler
    {
        private HttpStatusCode _status;
        public ResponseHandler() { }
        // Post Functions ----------
        public void Register(Socket clientSocket, string body)
        {
            var user = new User(null, GetUsername(body), GetPassword(body));
            try
            {
                user.Register();
                _status = HttpStatusCode.Created;
            }
            catch (DuplicateNameException e) // Username is taken
            {
                Console.WriteLine($"Registration failed: Username {e.Message} is taken\n");
                _status = HttpStatusCode.Conflict;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Registration failed: {e.Message}\n");
                _status = HttpStatusCode.NotFound;
            }
            // TODO make SendJsonRes... part of the ResHandler
            var response = new Response(_status);
            response.SendJsonResponse(clientSocket, _status.ToString());
        }
        public void LogIn(Socket clientSocket, string body)
        {
            var user = new User(null, GetUsername(body), GetPassword(body));
            try
            {
                user.LogIn();
                _status = HttpStatusCode.OK;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Login failed: {e.Message}\n");
                _status = HttpStatusCode.Unauthorized;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Login failed: {e.Message}\n");
                _status = HttpStatusCode.NotFound;
            }
            catch (InvalidCredentialException e)
            {
                Console.WriteLine($"Login failed: {e.Message}\n");
                _status = HttpStatusCode.Unauthorized;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Login failed: {e.Message}\n");
                _status = HttpStatusCode.NotFound;
            }
            var response = new Response(_status);
            response.SendJsonResponse(clientSocket, _status.ToString());
        }
        public void NewPackage(Socket clientSocket, string body)
        {
            try 
            {
                var user = new User(null, GetUsername(body), GetPassword(body));
                user.BuyPackage(); // TODO replace with CreatePackage()
                _status = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NotFound;
            }
        }
        public void AquirePackage(Socket clientSocket, string body)
        {
            try
            {
                
                var user = new User(null, GetUsername(body), GetPassword(body));
                body = JsonConvert.SerializeObject(user.BuyPackage());

            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Failed to aquire package: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Failed to aquire package: {e.Message}");
                _status = HttpStatusCode.Forbidden;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to aquire package: {e.Message}");
                _status = HttpStatusCode.NotFound;
            }        
        }
        public void Battles(Socket clientSocket, string body)
        {
        }
        public void Tradings(Socket clientSocket, string body)
        {
        }
        public void TradingsWithId(Socket clientSocket, string body)
        {
        }
        public void RetrieveUserData(Socket clientSocket, string body)
        {
        }
        public void ShowDeck(Socket clientSocket, string body)
        {
            var user = new User();
        }
        public void RetrieveStats(Socket clientSocket, string body)
        {
        }
        public void RetrieveScoreBoard(Socket clientSocket, string body)
        {
        }
        public void ShowCards(Socket clientSocket, string body)
        {
            var user = new User();
            List<Card> cards = user.ShowCards();
        }
        public void ShowTrades(Socket clientSocket, string body)
        {
        }
        public void NotFound(Socket clientSocket, string body)
        {
            _status = HttpStatusCode.NotFound;
            var response = new Response(_status);
            response.SendJsonResponse(clientSocket, _status.ToString());
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
