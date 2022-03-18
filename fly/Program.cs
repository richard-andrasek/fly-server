using System;
using System.Text;
using System.Text.RegularExpressions;
using fly.http;

/*
 * March 16, 2022
 * Richard Andrasek
 * This is a simple, ultra-lightweight http server to serve static files only
 * 
 * Temporary limitations (TODO):
 * - HTTP requests must be done in ASCII
 * - Maximum request size: 1400 bytes
 * 
 * Limitations:
 * - Only IPV4 support
 * - Query strings are ignored (static files only!)
 * - Server-side languages are not parsed (...static files only!)
 */

namespace fly
{
    class Program
    {
        static string UsageLine()
        {
            return 
                "   Usage:\n" +
                "       fly [-a address] [-p port]\n\n" +
                "   Options: \n" +
                "   -a address  This is the address to listen to for the http requests." +
                "               Default value: 127.0.0.1" +
                "   -p port     This is the port to listen to for the http requests." +
                "               Default value: 80";
        }
        static void Main(string[] args)
        {
            // Defaults
            string address = "127.0.0.1";
            int port = 80;

            int i = 0;
            while (i < args.Length - 1)
            {
                if(args[i] == "-a")
                {
                    address = args[i + 1].ToLower();
                    if (address != "localhost") {
                        Match reg = Regex.Match(address, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
                        if(!reg.Success)
                        {
                            Console.Out.WriteLine("Invalid ip address specified under -a option\n" + UsageLine());
                            return;
                        }
                    }
                    // Bump past the address
                    i++;
                }
                else if(args[i] == "-p")
                {
                    port = -1;
                    Int32.TryParse(args[i + 1], out port);
                    if (port < 10 || port > 65535)
                    {
                        Console.Out.WriteLine("Invalid port specified under -p option\n" + UsageLine());
                        return;
                    }
                    // Bump past the port
                    i++;
                }
                
                i++;
            }

            HttpServer server = new HttpServer(address, port);
            server.StartServer();
        }
   
    }
}
