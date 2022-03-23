using System;
using System.Collections.Generic;
using System.Text;
using fly.log;

namespace fly.http
{
    class HttpRequestParser
    {

        static public HttpRequest ParseRequest(string request_body)
        {
            // Initially strip out the \r so we're only dealing with Unix line endings...
            string unix_body = request_body.Replace("\r", "");

            string[] message_parts = unix_body.Split('\n');

            HttpRequest newRequest = new HttpRequest();

            ParseRequestLine(newRequest, message_parts[0]);

            int part = 1;
            for( ; part < message_parts.Length; part++)
            {
                // A blank line signals the end of the headers
                if(String.IsNullOrWhiteSpace(message_parts[part]))
                    break;

                ParseHeader(newRequest, message_parts[part]);
            }

            if (part < message_parts.Length)
            {
                StringBuilder sb = new StringBuilder();
                for(; part < message_parts.Length; part++)
                {
                    sb.Append(message_parts[part] + "\n");
                }
                newRequest.Body = sb.ToString();
            }
            return newRequest;
        }

        static private void ParseRequestLine(HttpRequest newRequest, string req)
        {
            // GET / HTTP/1.1
            string[] items = req.Split(' ');

            string meth = items[0];
            if (meth == "GET" || meth == "POST" || meth == "PUT" || 
                meth == "PATCH" || meth == "COPY" || meth == "DELETE")
            {
                // Unsupported: TRACE, LINK, UNLINK
                newRequest.Method = meth;
            }

            if (items.Length > 1)
                newRequest.Uri = items[1];
            if (items.Length > 2)
                newRequest.HttpVersion = items[2];
        }

        static private void ParseHeader(HttpRequest newRequest, string header)
        {
            int colon_i = header.IndexOf(':');
            // Invalid
            if (colon_i < 0)
            {
                Lumberjack logger = new Lumberjack("HttpRequestParser");
                logger.Warning("Found header with no colon: [" + header + "]");
                return;
            }
            string key = header.Substring(0, colon_i);
            string value = header.Substring(colon_i);
            newRequest.Headers[key] = value;
        }
    }
}
