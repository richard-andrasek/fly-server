using System;
using System.Collections.Generic;
using System.Text;
using fly.http_errors;
using fly.log;

namespace fly.http
{
    class HttpProcessor
    {
        public HttpProcessor() 
        {
            // Milestone 1: Parse request, send to appropriate command (Get/put/etc)
            // Milestone 2: Headers (CORS)
        }

        public byte[] Serve(string request)
        {
            Lumberjack logger = new Lumberjack("HttpProcessor");

            HttpRequest req = HttpRequestParser.ParseRequest(request);
            logger.Log("Request parsed.  Method: [" + req.Method + "]");

            // Make sure this is a request we can process (No FTP messages or anything crazy)
            HttpError error;
            if(!TryValidateHttpRequest(req, out error))
            {
                //Process the Error
                HttpErrorProcessor errorProcessor = new HttpErrorProcessor();
                HttpResponse error_output = errorProcessor.Serve(error);
                return error_output.ToResponseStream();
            }


            HttpResponse response;

            if(req.Method == "GET")
            {
                MethodGet handler = new MethodGet();
                response = handler.Serve(req);
            }
            // Unsupported methods:
            else
            {
                HttpErrorProcessor errorProcessor = new HttpErrorProcessor();
                response = errorProcessor.Serve(new NotImplemented("Invalid HTTP Method"));
            }

            return response.ToResponseStream();
        }

        private bool TryValidateHttpRequest(HttpRequest req, out HttpError error)
        {
            error = null;
            if (req.HttpVersion != "HTTP/1.1")
            {
                error = new NotImplemented("HTTP Version Not Supported");
                return false;
            }

            // If it's empty, either it was sent empty or it's a request type we don't process
            if (String.IsNullOrEmpty(req.Method))
            {
                error = new NotImplemented("Invalid HTTP Method");
                return false;
            }

            return true;
        }

    }
}
