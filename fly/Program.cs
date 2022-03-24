using System;
using System.Text;
using System.Text.RegularExpressions;
using fly.http;
using fly.config;
using System.IO;

/*
 * March 16, 2022
 * Richard Andrasek
 * This is a simple, ultra-lightweight http server to serve static files only
 * 
 * Temporary limitations (TODO):
 * - HTTP requests have not been tested with unicode filenames
 * 
 * Limitations:
 * - Only IPV4 support
 * - Query strings are ignored (static files only!)
 * - Server-side languages are not parsed (...static files only!)
 * - Only serves GET requests (No PATCH, PUT, POST, DELETE, TRACE, etc).
 */

namespace fly
{
    class Program
    {
        static string UsageLine()
        {
            return 
                "   Usage:\n" +
                "       fly [-a address] [-p port] [-d directory]\n\n" +
                "   Options: \n" +
                "   -a address   This is the address to listen to for the http requests.\n" +
                "                Default value: 127.0.0.1\n" +
                "   -p port      This is the port to listen to for the http requests.\n" +
                "                Default value: 80\n" +
                "   -d directory This is the root directory for the files to server\n" +
                "                Default value: current directory\n";
        }
        static void Main(string[] args)
        {
            // Defaults
            string host = Configuration.DefaultHost;
            int port = Configuration.DefaultPort;

            Console.Out.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());

            int i = 0;
            while (i < args.Length - 1)
            {
                if(args[i] == "-a")
                {
                    host = args[i + 1].ToLower();
                    if (host != "localhost") {
                        Match reg = Regex.Match(host, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
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
                else if (args[i] == "-d")
                {
                    string newDir = "";
                    newDir = args[i + 1];
                    if(!Directory.Exists(newDir))
                    {
                        Console.Out.WriteLine("Invalid directory specified under -d option\n" + UsageLine());
                        return;
                    }

                    Directory.SetCurrentDirectory(newDir);
                }
                
                i++;
            }

            HttpServer server = new HttpServer(host, port);
            server.StartServer();
        }
   
    }
}
