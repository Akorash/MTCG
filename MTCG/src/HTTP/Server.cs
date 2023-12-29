﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MTCG.src.Domain;
using MTCG.src.HTTP;

namespace MTCG.src.HTTP
{
    public class Server
    {
        private readonly int _port;
        private readonly int _maxConn;
        private readonly RequestHandler _dh; // Manages requests and sends the corresponding response

        public Server(int port, int maxConnections)
        {
            _port = port;
            _maxConn = maxConnections;

            _dh = new RequestHandler();
        }

        public async Task StartAsync()
        {
            try
            {
                Socket serverSocket = InitSocket(); // Bind and Listen
                Console.WriteLine($"Server is listening on port {_port} with a maximum of {_maxConn} connections...");
                while (true) // Listen and Accept --> Start new Task for each new client
                {
                    Socket clientSocket = await AcceptAsync(serverSocket);
                    _ = HandleClientAsync(clientSocket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }
        private async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                // Recieve message
                byte[] buffer = new byte[1024];
                int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                var reqStr = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                var req = new Request();

                // Parse Request
                req.Build(reqStr);

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
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void HandleGet(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":
                    _dh.RetrieveUserData(clientSocket, body);
                    break;
                case "/cards":
                    _dh.ShowCards(clientSocket, body);
                    break;
                case "/deck":
                    _dh.ShowDeck(clientSocket, body);
                    break;
                case "/stats":
                    _dh.RetrieveStats(clientSocket, body);
                    break;
                case "/scoreboard":
                    _dh.RetrieveScoreBoard(clientSocket, body);
                    break;
                case "/tradings":
                    _dh.ShowDeck(clientSocket, body);
                    break;
                default:
                    break;
            }
        }
        private void HandlePost(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users":
                    _dh.Register(clientSocket, body);
                    break;
                case "/sessions":
                    _dh.LogIn(clientSocket, body);
                    break;
                case "/packages":
                    
                    break;
                case "/transactions/packages":

                    break;
                case "/battles":

                    break;
                case "/tradings":

                    break;
                case "/tradings/{tradingdealid}":

                    break;
                default:
                    break;
            }
        }
        private void HandlePut(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":
                    
                    break;
                case "/deck":

                    break;
                default:
                    break;
            }
        }
        private void HandleDelete(Socket clientSocket, string url, string body)
        {
            switch (url)
            {
                case "/users/{username}":

                    break;
                case "/deck":

                    break;
                default:
                    break;
            }
        }

        private Socket InitSocket()
        {
            Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port));
            serverSocket.Listen(_maxConn);
            return serverSocket;
        }
        private async Task<Socket> AcceptAsync(Socket serverSocket)
        {
            return await Task.Factory.FromAsync(serverSocket.BeginAccept, serverSocket.EndAccept, null);
        }
    }
}
