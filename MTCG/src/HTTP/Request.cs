using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.src.HTTP
{

    // JsonSerializer
    internal class Request
    {
        private readonly char _splitWords = ' ';
        private readonly string _splitLines = "\r\n";
        private readonly int _linesUntilMethod = 3;
        private readonly int _methodIndex = 0;
        private readonly int _urlIndex = 0;
        public string Method { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }
        // Path
        // Params

        
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
            string[] reqLines = reqStr.Split(new[] { _splitLines }, StringSplitOptions.None);
            string reqLine = reqLines[0];
            string[] reqLineParts = reqLine.Split(_splitWords);

            // Parse Method and Url
            if (reqLineParts.Length == _linesUntilMethod)   // Check if request line has a valid format
            {
                Method = reqLineParts[_methodIndex];
                Url = reqLineParts[_urlIndex];
            }

            // Parsing Body 
            for (int i = 1; i < reqLines.Length; i++)   // Start with i = 1, since we're only interested in the lines after the request line 
            {
                string headerLine = reqLines[i];

                if (string.IsNullOrEmpty(headerLine))
                {
                    if (i + 1 < reqLines.Length)    // Check if there is anything after the empty line
                    {
                        Body = string.Join(_splitLines, reqLines, i + 1, reqLines.Length - (i + 1));
                    }
                }
            }
        }

        private void ParseMethodAndUrl(string method, string url)
        {

        }
    }
}
