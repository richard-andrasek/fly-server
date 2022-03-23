using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using fly.log;


namespace fly.tcp
{
    class TcpServer
    {
        TcpListener portListener = null;

        public TcpServer(IPAddress address, int port)
        {
            portListener = new TcpListener(address, port);
            
            // TODO: Milestones
            // Milestone 1: Threaded TCP listener (done)
            // Milestone 2: Create a thread pool to reduce threading time, improve stability
            // Milestone 3: Streaming content (MP4, MP3, etc)
        }

        public void RunServer()
        {
            Lumberjack logger = new Lumberjack("TcpServer");

            portListener.Start();

            try
            {
                while (true)
                {
                    // Wait for a new request
                    // Create a new thread to process the request
                    logger.Log("Waiting for a connection...");
                    TcpClient connection = portListener.AcceptTcpClient();
                    logger.Log("Connected!");

                    TcpThread t = new TcpThread();
                    t.Start(connection);
                }
            }
            catch (SocketException e)
            {
                // This is a server-level socket exception... 
                // This happens if the OS gives us a socket error, such as
                // being unable to connect to the socket.

                logger.Log("SocketException: " + e.ToString());
                portListener.Stop();
            }
        }

        
    }
}
