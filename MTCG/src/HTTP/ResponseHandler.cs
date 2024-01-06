using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.Domain.Entities;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;

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
        private Mapper _mapper;
        private BattleQueue _battleQueue;
        private readonly object _battleLock;

        public ResponseHandler(object battleLock, BattleQueue battleQueue) 
        {
            _status = default;
            _mapper = new();
            _battleLock = battleLock;
            _battleQueue = battleQueue;
        }

        //---------------------------------------------------------------------
        // /////////////////////////////// POST ///////////////////////////////
        //---------------------------------------------------------------------

        //------------------------- Authentification --------------------------
        public void Register(Socket clientSocket, string reqBody)
        {
            object body = reqBody; // If the registration is successful, return the request body (user)

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                user.Register();
                _status = HttpStatusCode.Created;
            }
            catch (DuplicateNameException e) // Username is taken
            {
                Console.WriteLine($"Registration failed: Username {e.Message} is taken\n");
                body = e.Message;
                _status = HttpStatusCode.Conflict;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Registration failed: {e.Message}\n");
                body = e.Message;
                _status = HttpStatusCode.NotFound;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        } 
        public void LogIn(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body
            
            try
            {
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(reqBody);
                var user = _mapper.Users.Map(userDTO);

                user.LogIn();
                _status = HttpStatusCode.OK;
                body = user; // description: User login successful
                // body content: string authentification token kienkoec-mtcgToken
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Login failed: {e.Message}\n");
                _status = HttpStatusCode.Unauthorized; // description: Invalid username/password provided
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
        public void NewPackage(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try 
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                user.CreatePackage();
                _status = HttpStatusCode.Created;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Unauthorized;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                if (e.Message.Contains("Not admin"))
                {
                    _status = HttpStatusCode.Forbidden; 
                }
                else if (e.Message.Contains("At least one card in the package already exists"))
                {
                    _status = HttpStatusCode.Conflict; 
                }
                _status = HttpStatusCode.Forbidden;          
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NotFound;
            }
        }
        public void AquirePackage(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body
            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                body = user.BuyPackage();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Failed to aquire package due to unauthorized access: {e.Message}");
                _status = HttpStatusCode.Unauthorized; //401
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Failed to aquire package due to insufficient coins: {e.Message}");
                _status = HttpStatusCode.Forbidden;//403 Not enough money
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to aquire package: {e.Message}");
                _status = HttpStatusCode.NotFound; // No card package availabe for buying
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void Battle(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try 
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                user.Battle();
                _status = HttpStatusCode.OK; // return battle log
            }
            catch (UnauthorizedAccessException e) 
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
               _status = HttpStatusCode.Unauthorized;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void NewTradingDeal(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                _status = HttpStatusCode.Created;
            }
            catch (InvalidDataException e) // No trading deals available
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NoContent;
            }
            catch (ArgumentException e) // Deal contains a card that is not owned by the user or is blocked in deck
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Forbidden;
            }
            catch (InvalidOperationException e) // A deal with this id already exists
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Conflict;
            }
            catch (UnauthorizedAccessException e) // Unauthorized error
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Unauthorized;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NotFound;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void Trade(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                _status = HttpStatusCode.Created;
            }
            catch (InvalidDataException e) // No trading deals available
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NoContent;
            }
            catch (ArgumentException e) // Deal contains a card that is not owned by the user or is blocked in deck
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Forbidden;
            }
            catch (InvalidOperationException e) // A deal with this id already exists
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Conflict;
            }
            catch (UnauthorizedAccessException e) // Unauthorized error
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.Unauthorized;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _status = HttpStatusCode.NotFound;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
        // /////////////////////////////// GET ////////////////////////////////
        //---------------------------------------------------------------------
        public void RetrieveUserData(Socket clientSocket, string reqBody) // TODO
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                // Admin auth

                user.Battle();
                _status = HttpStatusCode.OK; // 200
            }
            // 401 Unauthorized
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }
            // 404 Not found

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowCards(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                body = user.ShowCards();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e) // 204 user has no cards
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }
            //401

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowDeck(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                body = user.ShowDeck();
                _status = HttpStatusCode.OK;
            }
            // 204
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void RetrieveStats(Socket clientSocket, string reqBody) // TODO
        {
            object body = default; // Response body

            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                user.Battle();
                _status = HttpStatusCode.OK; // body = userstats
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);

        }
        public void RetrieveScoreBoard(Socket clientSocket, string reqBody) // TODO // just bearer token... for most of these
        {
            object body = default; // Response body
            // Retrieves the user scoreboard ordered by the user's ELO.
            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

                user.Battle();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Battle attempt falied: {e.Message}");
                _status = HttpStatusCode.Unauthorized;
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowTradingDeals(Socket clientSocket, string reqBody)
        {
            object body = default; // Response body
            try
            {
                var bearerToken = JsonConvert.DeserializeObject<BearerTokenDTO>(reqBody);
                var user = new User(bearerToken.BearerToken);

            }
            catch (Exception e)
            {
                // 204 No trading deals available
                Console.WriteLine(e.Message);
            }
            // 401 unauth error

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
        // /////////////////////////////// PUT ////////////////////////////////
        //---------------------------------------------------------------------

        public void UpdateUserData(Socket clientSocket, string body)
        {
            // Uptates and Retrieves user data
            // 200 description: User sucessfully updated.
            // 401
            // 404 description: User not found.
        }
        public void ConfigureDeck(Socket clientSocket, string body)
        {
            // gets four card uuids
            // failed request doesnt change the deck
            // 200
            // 400 not the requiremed amount of cards
            // 401 unauthorized error
            // 403 at least oneof the cards doesnt belogn to the user or is not available
        }
        //---------------------------------------------------------------------
        // ///////////////////////////// DELETE ///////////////////////////////
        //---------------------------------------------------------------------
        public void DeleteTradingDeal(Socket clientSocket, string body)
        {
            //200
            // 401 Unauthorized error
            // 403 card not owned by the user
            // 404 // not found
            // deal with this id already exsts
        }
        //---------------------------------------------------------------------
        public void NotFound(Socket clientSocket, string body)
        {
            _status = HttpStatusCode.NotFound;
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
    }
}
