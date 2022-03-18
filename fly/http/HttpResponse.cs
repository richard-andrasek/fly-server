using System;
using System.Collections.Generic;
using System.Text;
using fly.config;

namespace fly.http
{
    class HttpResponse
    {
        public HttpResponse()
        {
            this.ResponseCode = 200;
            this.Headers = new Dictionary<string, string>();
            this.Headers["Server"] = "Fly " + Configuration.ServerVersion;
        }

        public int ResponseCode { get; set; }    // eg. 200
        public string ResponseCodeText { get; set; } // eg "OK"
        public string HttpVersion { get { return "HTTP/1.1"; } }
        public byte[] BinaryBody { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public void SetBody(string body)
        {
            BinaryBody = System.Text.Encoding.ASCII.GetBytes(body);
        }

        public byte[] ToResponseStream()
        {
            StringBuilder outMessage = new StringBuilder();

            // Status Line  (HTTP/1.1 200 OK)
            outMessage.Append(HttpVersion);
            outMessage.Append(" ");
            outMessage.Append(ResponseCode.ToString());
            outMessage.Append(" ");
            outMessage.AppendLine(ResponseCodeText);

            // Headers
            foreach (KeyValuePair<string, string> item in Headers)
            {
                outMessage.Append(item.Key);
                outMessage.Append(": ");
                outMessage.AppendLine(item.Value);
            }

            // Empty line to signal the end of the headers
            outMessage.AppendLine();

            string asciiOut = outMessage.ToString();

            // Body
            byte[] outData;
            if (BinaryBody == null || BinaryBody.Length == 0)
            {
                // As is, no body
                outData = System.Text.Encoding.ASCII.GetBytes(asciiOut);
            }
            else
            {
                // Convert what we have to bytes
                byte[] headerData = System.Text.Encoding.ASCII.GetBytes(asciiOut);

                // Then append our binary data
                outData = new byte[headerData.Length + BinaryBody.Length];
                Buffer.BlockCopy(headerData, 0, outData, 0, headerData.Length);
                Buffer.BlockCopy(BinaryBody, 0, outData, headerData.Length, BinaryBody.Length);
            }

            return outData;
        }
    }
}
