using System;
using System.Collections.Generic;
using System.Text;
using fly.filesystem;
using fly.http_errors;

namespace fly.http
{
    class MethodGet : IHttpMethod
    {
        public HttpResponse Serve(HttpRequest req)
        {
            HttpResponse response = new HttpResponse();

            HttpError httpError;
            FlyFile file;
            if (!FlyFileSystem.TryRetrieveByURI(req.Uri, out file, out httpError))
            {
                HttpErrorProcessor proc = new HttpErrorProcessor();
                return proc.Serve(httpError);
            }

            response.ResponseCode = 200;
            response.ResponseCodeText = "OK";
            response.BinaryBody = file.FileContent;

            response.Headers["Content-Type"] = file.ContentType;
            //if(file.ContentEncoding != "ascii")
            //{
            //    response.Headers["Content-Encoding"] = file.ContentEncoding;
            //}
            response.Headers["Content-Length"] = response.BinaryBody.Length.ToString();

            return response;
        }
    }
}
