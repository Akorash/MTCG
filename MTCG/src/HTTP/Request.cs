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
        private readonly int _startLineElements = 3;
        private readonly int _methodIndex = 0;
        private readonly int _urlIndex = 1;
        public string Method { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }

        public Request()
        {
            Method = string.Empty;
            Url = string.Empty;
            Body = string.Empty;
        }

        public void Build(string message)
        {
            try
            {
                ParseRequest(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }   
        }

        private void ParseRequest(string request)
        {
            // Split request into lines
            string[] requestLines = request.Split(new[] { _splitLines }, StringSplitOptions.None);  // Split request into lines
            
            // Parse start line
            string startLine = requestLines[0];
            string[] startLineParts = startLine.Split(_splitWords);
            ParseMethodAndUrl(startLineParts[0], startLineParts[1], startLineParts.Length);

            // Parse body 
            ParseBody(requestLines);
        }
        private void ParseMethodAndUrl(string method, string url, int amountOfElementsInStartLine)
        {
            if (amountOfElementsInStartLine < _startLineElements || amountOfElementsInStartLine > _startLineElements)
            {
                throw new ArgumentException();
            }
            Method = method;
            Url = url;
        }
        private void ParseBody(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length; i++)   // Starts with i = 1 since we've already looked at the start line 
            {
                string currentLine = requestLines[i];

                if (string.IsNullOrEmpty(currentLine))
                {
                    if (i + 1 < requestLines.Length)    // i contains the index of the empty line, if i+1 is not a valid index, there is nothing after the empty line
                    {
                        Body = string.Join(_splitLines, requestLines, i + 1, requestLines.Length - (i + 1));
                    }
                }
            }
        }
    }
}
