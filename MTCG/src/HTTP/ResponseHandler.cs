using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.ComponentModel;
using System.Security.Authentication;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.InteropServices.ObjectiveC;

using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.Domain.Entities;
using MTCG.src.Domain;
using MTCG.src.HTTP;

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
        private BattleQueue _battleQueue;
        private readonly object _battleLock;
        public ResponseHandler() { }
        public ResponseHandler(object battleLock, BattleQueue battleQueue) 
        {
            _battleLock = battleLock;
            _battleQueue = battleQueue;
        }
        //---------------------------------------------------------------------
        // /////////////////////////////// POST ///////////////////////////////
        //---------------------------------------------------------------------

        //------------------------- Authentification --------------------------
        public void Register(Socket clientSocket, string reqBody)
        {
            Console.WriteLine($"Debug: {reqBody}");
            var user = JsonConvert.DeserializeObject<User>(reqBody);

            // var user = new User(null, GetUsername(reqBody), GetPassword(reqBody));
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
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, user);
        }
        public void LogIn(Socket clientSocket, string reqBody)
        {
            object body = null; // Response body

            var user = JsonConvert.DeserializeObject<User>(reqBody);
            try
            {
                user.LogIn();
                _status = HttpStatusCode.OK;
                body = user;
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
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
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
        public void AquirePackage(Socket clientSocket, string reqBody)
        {
            object body = null; // Response body
            try
            {
                var user = JsonConvert.DeserializeObject<User>(reqBody);
                body = user.BuyPackage();
                Console.WriteLine(body);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Failed to aquire package due to unauthorized access: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Failed to aquire package due to insufficient coins: {e.Message}");
                _status = HttpStatusCode.Forbidden;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to aquire package: {e.Message}");
                _status = HttpStatusCode.NotFound;
            }
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void Battle(Socket clientSocket, string reqBody)
        {
            try 
            {
                var user = JsonConvert.DeserializeObject<User>(reqBody); L

                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e) 
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
               _status = HttpStatusCode.Unauthorized;
            }

            /*
      summary: Enters the lobby to start a battle.
            
      description: If there is already another user in the lobby, the battle will start immediately. 
            Otherwise the request will wait for another user to enter.
      responses:
        '200':
          description: The battle has been carried out successfully.
          content:
            text/plain:
              schema:
                type: string
                description: The battle log.
             */
        }
        public void Tradings(Socket clientSocket, string body)
        {
        }
        public void TradingsWithId(Socket clientSocket, string body)
        {
        }
        //---------------------------------------------------------------------
        // /////////////////////////////// GET ////////////////////////////////
        //---------------------------------------------------------------------
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
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
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
