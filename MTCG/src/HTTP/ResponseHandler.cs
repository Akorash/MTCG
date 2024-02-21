using Microsoft.Extensions.FileProviders.Physical;
using MTCG.src.DataAccess.Persistance;
using MTCG.src.DataAccess.Persistance.DTOs;
using MTCG.src.DataAccess.Persistance.Mappers;
using MTCG.src.DataAccess.Persistance.Repositories;
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

        public ResponseHandler() 
        {
            _status = default;
            _mapper = new();
        }

        //---------------------------------------------------------------------
        // /////////////////////////////// POST ///////////////////////////////
        //---------------------------------------------------------------------

        //------------------------- Authentification --------------------------
        public void Register(Socket clientSocket, string auth, string reqBody)
        {
            object body = null; // Response body

            try
            {
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(reqBody);
                var user = _mapper.Users.Map(userDTO);
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

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
        public void LogIn(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body
            
            try
            {
                var userDTO = JsonConvert.DeserializeObject<UserDTO>(reqBody);
                var user = _mapper.Users.Map(userDTO);

                body = user.LogIn();
                _status = HttpStatusCode.OK;
            }
            catch (ArgumentException e) // Invaid username/password
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
        public void NewPackage(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try 
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);
                
                body = user.CreatePackage(user.Id);
                _status = HttpStatusCode.Created;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine(e.Message);
            }
            catch (HttpRequestException e)
            {
                if (e.Message.Contains("Not admin"))
                {
                    _status = HttpStatusCode.Forbidden; 
                }
                else if (e.Message.Contains("At least one card in the package already exists"))
                {
                    _status = HttpStatusCode.Conflict; 
                }
                _status = HttpStatusCode.Forbidden;
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine(e.Message);
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void AquirePackage(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                body = user.BuyPackage();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to aquire package due to unauthorized access: {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                _status = HttpStatusCode.Forbidden;// 403 Not enough money
                Console.WriteLine($"Failed to aquire package due to insufficient coins: {e.Message}");
            }
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound; // No card package availabe for
                Console.WriteLine($"Failed to aquire package: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void Battle(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try 
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                user.Battle(); // Add user to the battle queue
                // Wait until someone else has joined and the battle has ended
                _status = HttpStatusCode.OK; 
            }
            catch (UnauthorizedAccessException e) 
            {
               _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Battle attempt falied: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void NewTradingDeal(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

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
        public void Trade(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                var card = JsonConvert.DeserializeObject<CardDTO>(reqBody);
                var other = JsonConvert.DeserializeObject<UserDTO>(reqBody);

                // body = user.Trade(_mapper.Cards.Map(card), _mapper.Users.Map(other));
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
        public void RetrieveUserData(Socket clientSocket, string auth, string reqBody) // TODO
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                var other = JsonConvert.DeserializeObject<UserDTO>(reqBody);

                body = user.ViewUserData(other.Username);
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Falied to retrieve user data: {e.Message}");
            }
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Falied to retrieve user data: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowCards(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                if (user == null)
                {
                    _status = HttpStatusCode.Unauthorized;
                }
                else 
                {
                    body = user.ShowCards();
                    _status = HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("No cards"))
                {
                    _status = HttpStatusCode.NoContent;
                }
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Failed to retrieve cards: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowDeck(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                body = user.ShowDeck();
                _status = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("No cards"))
                {
                    _status = HttpStatusCode.NoContent;
                }
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to retrieve deck: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void RetrieveStats(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                body = user.ViewStats();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to retrieve stats: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to retrieve stats: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void RetrieveScoreBoard(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                body = user.ViewScoreBoard();
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Falied to retrieve scoreboard: {e.Message}");
            }
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Falied to retrieve scoreboard: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ShowTradingDeals(Socket clientSocket, string auth, string reqBody)
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                body = user.ShowTradingDeals();
                _status = HttpStatusCode.OK;   
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to retrieve trading deals: {e.Message}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("No trading deals"))
                {
                    _status = HttpStatusCode.NoContent;
                }
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Failed to retrieve trading deals: {e.Message}");
            }

            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
        // /////////////////////////////// PUT ////////////////////////////////
        //---------------------------------------------------------------------

        public void UpdateUserData(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                var other = JsonConvert.DeserializeObject<UserDTO>(reqBody);

                // body = user.UpdateData(_mapper.Users.Map(other));
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to update user: {e.Message}");
            }
            catch (Exception e) 
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Failed to update user: {e.Message}");
            }
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        public void ConfigureDeck(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body

            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                var deckDTO = JsonConvert.DeserializeObject<DeckDTO>(reqBody);

                var deck = new List<Guid>() { deckDTO.Card1Id, deckDTO.Card2Id, deckDTO.Card3Id, deckDTO.Card4Id };
                body = user.ConfigureDeck(deck);
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to reconfigure deck: {e.Message}");
            }
            // TODO: Add case 400 --> Not the required amount of cards
            // TODO: Add case 403 --> At least oneof the cards doesnt belogn to the user or is not available
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Failed to reconfigure deck: {e.Message}");
            }
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
        // ///////////////////////////// DELETE ///////////////////////////////
        //---------------------------------------------------------------------
        public void DeleteTradingDeal(Socket clientSocket, string auth, string reqBody) // Unfinished
        {
            object body = default; // Response body
            try
            {
                if (auth == null || auth == string.Empty)
                {
                    throw new ArgumentNullException();
                }
                var user = new User(auth);

                var deckDTO = JsonConvert.DeserializeObject<DeckDTO>(reqBody);

                var deck = new List<Guid>() { deckDTO.Card1Id, deckDTO.Card2Id, deckDTO.Card3Id, deckDTO.Card4Id };
                body = user.ConfigureDeck(deck);
                _status = HttpStatusCode.OK;
            }
            catch (UnauthorizedAccessException e)
            {
                _status = HttpStatusCode.Unauthorized;
                Console.WriteLine($"Failed to delete trading deal: {e.Message}");
            }
            // TODO: Add case 400 --> Not the required amount of cards
            // TODO: Add case 403 --> Card not owned by the user
            catch (Exception e)
            {
                _status = HttpStatusCode.NotFound;
                Console.WriteLine($"Failed to delete trading deal: {e.Message}");
            }
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
        //---------------------------------------------------------------------
        public void NotFound(Socket clientSocket, string auth, string reqBody)
        {
            object body = null;
            _status = HttpStatusCode.NotFound;
            var response = new Response();
            response.SendJsonResponse(clientSocket, _status, body);
        }
    }
}
