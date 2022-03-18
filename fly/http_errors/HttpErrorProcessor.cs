using System;
using System.Collections.Generic;
using System.Text;
using fly.log;
using fly.http;

namespace fly.http_errors
{
    class HttpErrorProcessor
    {
        public HttpErrorProcessor()
        {
            // TODO: Milestones
            // Milestone 1:  Convert error into http output (Done)
            // Milestone 2:  Improve default HTML to serve along with the error
            // Milestone 3:  Add Actual Headers
            // Milestone 4:  Allow for override of default HTML with a 404.html page
        }


        public HttpResponse Serve(HttpError error)
        {
            Lumberjack logger = new Lumberjack("HttpErrorProcessor");
            StringBuilder output = new StringBuilder();

            // Response line
            logger.Log("Sending Error Code: [" + error.ToString() + "]");

            HttpResponse newResponse = new HttpResponse();
            newResponse.ResponseCode = error.ErrorCode;
            newResponse.ResponseCodeText = error.ErrorName;

            // Headers
            newResponse.Headers["Content-type"] = "text/html";

            // Message body
            string str_errNumber = error.ErrorCode.ToString();
            output.Append("<html><head><title>Fly Server - Error ");
            output.Append(str_errNumber);
            output.Append("</title></head>\n<body style='padding: 50;'>\n<h2>Fly Server Error</h2>\n");
            output.Append("<p>An error was recieved by the Fly server.</p>\n");
            output.Append("<h3>Error Code: ");
            output.Append(str_errNumber);
            output.Append(" (");
            output.Append(error.ErrorName);
            output.Append(")</h3>\n<h4>");
            output.Append(error.ErrorString);
            output.Append("</h4>\n</body></html>");

            newResponse.SetBody(output.ToString());
            return newResponse;
        }


    }
}
