using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MTCG.src.Domain;

namespace MTCG.src.HTTP
{
    public class Server
    {
        private readonly int _port;
        private readonly int _maxConn;
        private readonly RequestHandler _dh;

        public Server(int port, int maxConnections)
        {
            this._port = port;
            this._maxConn = maxConnections;
            this._dh = new();
        }

        public async Task StartAsync()
        {
            try {
                // Bind and listen to incoming clients
                Socket serverSocket = InitSocket();
                Console.WriteLine($"Server is listening on port {_port} " +
                                  $"with a maximum of {_maxConn} connections...");
                while (true) {
                    Socket clientSocket = await AcceptAsync(serverSocket);
                    _ = HandleClientAsync(clientSocket);
                }  
            }
            catch (Exception e) {
                Console.WriteLine($"{e.Message}");
            }
        }
        private async Task HandleClientAsync(Socket clientSocket)
        {
            // TODO: Buffer size error
            byte[] buffer = new byte[1024];
            // TODO: Error handling?
            int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            string reqStr = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Request req = new(reqStr);

            switch (req.Method)
            {
                case "GET":
                    HandleGet(clientSocket, req.Url, req.Body);
                    break;
                case "POST":
                    HandlePost(clientSocket, req.Url, req.Body);
                    break;
                case "PUT":
                    HandlePut(clientSocket, req.Url, req.Body);
                    break;
                case "DELETE":
                    HandleDelete(clientSocket, req.Url, req.Body);
                    break;
                default:
                    HandleNotFoundRequest(clientSocket);
                    break;
            }
        }
        private void HandleGet(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":
                    // _dh.RetrieveData()
                    SendJsonResponse(clientSocket, "users/id");
                    break;
                case "/cards":
                    // _dh.ShowCards()
                    SendJsonResponse(clientSocket, "cards");
                    break;
                case "/deck":
                    // _dh.ShowDeck()
                    SendJsonResponse(clientSocket, "deck");
                    break;
                case "/stats":
                    // _dh.RetrieveStats()
                    SendJsonResponse(clientSocket, "stats");
                    break;
                case "/scoreboard":
                    // _dh.RetrieveScoreboard
                    SendJsonResponse(clientSocket, "scoreboard");
                    break;
                case "/tradings":
                    // _dh.ShowTrades
                    SendJsonResponse(clientSocket, "tradings");
                    break;
                default:
                    SendJsonResponse(clientSocket, "default");
                    break;
            }
        }
        private void HandlePost(Socket clientSocket, string url, string body)
        {
            // var requestData = JsonConvert.DeserializeObject<MyDataModel>(jsonRequest);
            switch (url)
            {
                case "/users":
                    // Register a new user with uname and password
                    // 201 User successfully created, 409 User with same username already registered
                    // _dh.RegisterUser()
                    SendJsonResponse(clientSocket, "users");
                    break;
                case "/sessions":
                    SendJsonResponse(clientSocket, _dh.Post.Login(body));
                    break;
                case "/packages":
                    // Create new card package from an array of cards, only admin
                    // 201 Package and cards successfully created
                    // 401 $ref: '#/components/responses/UnauthorizedError'
                    // 403 Provided user is not "admin"
                    // 409 At least one card in the packages already exists
                    SendJsonResponse(clientSocket, "packages");
                    break;
                case "/transactions/packages":
                    // Aquire a card package with user money
                    // 200 A package has been successfully bought
                    // 401 $ref: '#/components/responses/UnauthorizedError'
                    // 403 Not enough money for buying a card package
                    // 404 No card package available for buying
                    SendJsonResponse(clientSocket, "transactons/packages");
                    break;
                case "/battles":
                    SendJsonResponse(clientSocket, "battles");
                    break;
                case "/tradings":
                    SendJsonResponse(clientSocket, "tradings");
                    break;
                case "/tradings/{tradingdealid}":
                    SendJsonResponse(clientSocket, "tradings/id");
                    break;
                default:
                    SendJsonResponse(clientSocket, "default");
                    break;
            }
        }
        private void HandlePut(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":
                    SendJsonResponse(clientSocket, "users");
                    break;
                case "/deck":
                // Configures the deck with four provided cards
                // Send four card IDs to configure a new full deck. A failed request will not change the previously defined deck.
                // '200':description: The deck has been successfully configured
                // '400':description: The provided deck did not include the required amount of cards
                // ' 401' $ref: '#/components/responses/UnauthorizedError'
                // '403':
                // description: At least one of the provided cards does not belong to the user or is not available.
                    SendJsonResponse(clientSocket, "deck");
                    break;
                default:
                    SendJsonResponse(clientSocket, "default");
                    break;
            }
        }
        private void HandleDelete(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":
                    SendJsonResponse(clientSocket, "users/id");
                    break;
                case "/deck":
                    SendJsonResponse(clientSocket, "deck");
                    break;
                default:
                    SendJsonResponse(clientSocket, "default");
                    break;
            }
        }

        private Socket InitSocket()
        {
            //try
            //{
            Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port));
            serverSocket.Listen(_maxConn);
            return serverSocket;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"{e.Message}");
            //    return null;
            //}
        }
        private async Task<Socket> AcceptAsync(Socket serverSocket)
        {
            return await Task.Factory.FromAsync(serverSocket.BeginAccept, serverSocket.EndAccept, null);
        }
        private void HandleNotFoundRequest(Socket clientSocket)
        {
            // Respond with a 404 Not Found error
            string response = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\nNot Found";
            byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
            SendResponse(clientSocket, responseBuffer);
        }
        private void SendJsonResponse(Socket clientSocket, string message)
        {
            var responseData = new { Message = message };
            string jsonResponse = JsonConvert.SerializeObject(responseData);
            byte[] responseBuffer = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n{jsonResponse}");
            SendResponse(clientSocket, responseBuffer);
        }
        private void SendResponse(Socket clientSocket, byte[] responseBuffer)
        {
            clientSocket.Send(responseBuffer);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
