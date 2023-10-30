using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.HTTP
{
    internal class Request
    {
        public string Method { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }
        
        // IDEA: AllowedMethods

        public Request(string message)
        {
            Method = string.Empty;
            Url = string.Empty;
            Body = string.Empty;

            HandleRequest(message);
        }

        private void HandleRequest(string reqStr)
        {
            // Split Request String into Lines
            string[] reqLines = reqStr.Split(new[] { "\r\n" }, StringSplitOptions.None);
            string reqLine = reqLines[0];
            string[] reqLineParts = reqLine.Split(' ');

            // Parse Method and Url
            if (reqLineParts.Length == 3)   // Check if request line has a valid format
            {
                Method = reqLineParts[0];
                Url = reqLineParts[1];
            }

            // Parsing Body 
            for (int i = 1; i < reqLines.Length; i++)   // Start with i = 1, since we're only interested in the lines after the request line 
            {
                string headerLine = reqLines[i];

                if (string.IsNullOrEmpty(headerLine))
                {
                    if (i + 1 < reqLines.Length)    // Check if there is anything after the empty line
                    {
                        Body = string.Join("\r\n", reqLines, i + 1, reqLines.Length - (i + 1));
                    }
                }
            }
        }
    }
}
