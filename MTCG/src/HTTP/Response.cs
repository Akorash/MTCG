using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.HTTP
{
    public class Response
    {
        private string _version = "1.1";
        private string _protocol = "HTTP";
        private string _status;
        private string _body; 
        public Response() 
        {
            _status = default; 
            _body = default;
        }
        public void Build(string message) 
        {
            /*
            ParseStatusLine(message);
            */
        }
        private void SendJsonResponse(Socket clientSocket, string message)
        {
            var responseData = new { Message = message };
            string jsonResponse = JsonConvert.SerializeObject(responseData);
            byte[] responseBuffer = Encoding.ASCII.GetBytes(_protocol + "/" + _version + jsonResponse);
            SendResponse(clientSocket, responseBuffer);
        }
        private void SendResponse(Socket clientSocket, byte[] responseBuffer)
        {
            clientSocket.Send(responseBuffer);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        /*
        private void ParseStatusLine(string message)
        {
            if (amountOfElementsInStartLine <= _startLineElements || amountOfElementsInStartLine >= _startLineElements)
            {
                throw new ArgumentException();
            }
            Method = method;
            Url = url;
        }
        */
    }
}
