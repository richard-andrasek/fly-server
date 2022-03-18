using System;
using System.Collections.Generic;
using System.Text;

namespace fly.http
{
    class HttpRequest
    {
        public HttpRequest()
        {
            this.Headers = new Dictionary<string, string>();
            this.Body = "";
            this.Method = "";
            this.Uri = "";
            this.HttpVersion = "";
        }

        public string Method { get; set; }
        public string Uri { get; set; }
        public string HttpVersion { get; set; }
        public Dictionary<string, string> Headers { get; }
        public string Body { get; set; }
        
    }
}
