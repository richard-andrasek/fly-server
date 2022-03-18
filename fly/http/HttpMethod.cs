using System;
using System.Collections.Generic;
using System.Text;

namespace fly.http
{
    interface IHttpMethod
    {
        public HttpResponse Serve(HttpRequest req);
    }
}
