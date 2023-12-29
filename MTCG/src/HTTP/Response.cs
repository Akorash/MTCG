using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace MTCG.src.HTTP
{
    public class Response
    {
        private readonly string _newLine = "\r\n";
        private readonly string _version = "1.1";
        private readonly string _protocol = "HTTP";
        private readonly string _headerContentType = $"Content-Type: application/json";
        private string _headerContentLength;
        private HttpStatusCode _status;
        public Response(HttpStatusCode status)
        {
            _status = status;
            _headerContentLength = default;
        }
        public void SendJsonResponse(Socket clientSocket, string message)
        {
            // Object to json
            var responseData = new { Message = message };
            string jsonResponse = JsonConvert.SerializeObject(responseData);

            // Set content length
            _headerContentLength = $"Content-Length: {jsonResponse.Length}";

            // Send response
            byte[] responseBuffer = Encoding.ASCII.GetBytes($"{_protocol}/{_version} {((int)_status)}{_newLine}" +
                                                            $"{_headerContentType}{_newLine}{_headerContentLength}" +
                                                            $"{_newLine}{_newLine}{jsonResponse}");
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
