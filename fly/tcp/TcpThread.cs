using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using fly.log;
using fly.http;

namespace fly.tcp
{
    class TcpThread
    {
        readonly Thread thread;
        private const int MAX_PACKET = 1400;
        private Lumberjack logger;
        public TcpThread()
        {
            thread = new Thread(RequestLoop);

            // TODO: Current limitations
            // * This has not been tested on unicode requests (unicode file names)

            logger = new Lumberjack("TcpThread");
        }

        public void Start()
        {
            thread.Start();
        }


        private void RequestLoop()
        {
            while (true)
            {
                if (TcpServer.ConnectionQueue.TryDequeue(out TcpClient client))
                {
                    ProcessClientRequest(client);
                }
                // TODO: Make this an event rather than a sleep loop.
                Thread.Sleep(1);
            }
        }

        
        private void ProcessClientRequest(TcpClient client)
        {
            var stream = client.GetStream();
            string imei = String.Empty;

            string data = null;
            Byte[] bytes = new Byte[MAX_PACKET+1]; // +1 is probably not needed...

            logger.Log("Processing request from ip: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            try
            {
                int bytesRead = stream.Read(bytes, 0, MAX_PACKET);

                // Currently we have a limitation of 1400 bytes.
                // This is not a problem because we only server GET requests. The bits that are actually important are
                // in the first 15-20 bytes.  (Method, URL, HTTP Version).
                // Also, any web requests coming in with > 1400 bytes in headers aren't relevant because the
                // headers aren't needed for static files. 
                if (bytesRead == MAX_PACKET)
                {
                    logger.Error("Received more than " + MAX_PACKET.ToString() + " bytes.  Message truncated.");
                }

                // Convert the bytes to ASCII for the web request
                data = Encoding.ASCII.GetString(bytes, 0, bytesRead);

                {
                    int firstLineIndex = data.IndexOf('\n');
                    if (firstLineIndex <= 0)
                        firstLineIndex = bytesRead;
                    logger.Log("Received " + bytesRead.ToString() + " bytes: [" + data.Substring(0, firstLineIndex - 1) + "]");
                }

                // Send the request to the HTTP processor
                var http = new HttpProcessor();
                byte[] response = http.Serve(data);

                // Convert the return string back to bytes
                //string logstring = Encoding.ASCII.GetString(response);
                logger.Log("Sending " + response.Length + " bytes");
                stream.Write(response, 0, response.Length);
                stream.Close();
            }
            catch (Exception e)
            {
                logger.Log("Exception: " + e.ToString());
                client.Close();
            }
        }
    }
}
