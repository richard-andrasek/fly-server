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
        public TcpThread()
        {
            thread = new Thread(new ParameterizedThreadStart(TcpThread.ProcessRequest));
            // TODO: Current limitations
            // * This only supports ASCII
            // * This only supports requests up to MAX_PACKET
        }

        public void Start(TcpClient client)
        {
            thread.Start(client);
        }
        
        private static void ProcessRequest(Object obj)
        {
            Lumberjack logger = new Lumberjack("TcpThread");

            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            string imei = String.Empty;

            string data = null;
            Byte[] bytes = new Byte[MAX_PACKET+1]; // +1 is probably not needed...

            try
            {
                int bytesRead = stream.Read(bytes, 0, MAX_PACKET);

                // TODO: Need to fix this limitation this because... POST can easily exceed MAX_PACKET (let alone GET headers)
                if (bytesRead == MAX_PACKET)
                {
                    logger.Error("Received more than " + MAX_PACKET.ToString() + " bytes.  Message truncated.");
                }

                // Convert the bytes to ASCII for the web request
                data = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                logger.Log("Received " + bytesRead.ToString() + " bytes: [" + data + "]");

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
