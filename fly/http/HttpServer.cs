using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace fly.http
{
    class HttpServer
    {
        /*
         * For reference:
         * https://www.w3.org/Protocols/HTTP/1.1/draft-ietf-http-v11-spec-01.html
         */
        IPAddress http_address;
        int http_port;
        public HttpServer(string address, int port)
        {
            http_address = IPAddress.Parse(address);
            http_port = port;
        }

        public void StartServer()
        {
            // This is effectively a wrapper for the TCP Server.
            var server = new fly.tcp.TcpServer(http_address, http_port);

            server.RunServer();
        }
    }
}
