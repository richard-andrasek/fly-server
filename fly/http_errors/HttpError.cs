using System;
using System.Collections.Generic;
using System.Text;

namespace fly.http_errors
{
    abstract public class HttpError
    {
        public int ErrorCode { get; set; }
        public string ErrorName { get; set; }
        public string ErrorString { get; set; }

        public override string ToString()
        {
            return ErrorCode + " (" + ErrorName + "): " + ErrorString;
        }
    }
    /*
     *        Status-Code    = "100"   ; Continue
                      | "101"   ; Switching Protocols
                      | "200"   ; OK
                      | "201"   ; Created
                      | "202"   ; Accepted
                      | "203"   ; Non-Authoritative Information
                      | "204"   ; No Content
                      | "205"   ; Reset Content
                      | "206"   ; Partial Content
                      | "300"   ; Multiple Choices
                      | "301"   ; Moved Permanently
                      | "302"   ; Moved Temporarily
                      | "303"   ; See Other
                      | "304"   ; Not Modified
                      | "305"   ; Use Proxy
    -------------------------------ERRORS:
                      | "400"   ; Bad Request
                      | "401"   ; Unauthorized
                      | "402"   ; Payment Required
                      | "403"   ; Forbidden
                      | "404"   ; Not Found
                      | "405"   ; Method Not Allowed
                      | "406"   ; None Acceptable
                      | "407"   ; Proxy Authentication Required
                      | "408"   ; Request Timeout
                      | "409"   ; Conflict
                      | "410"   ; Gone
                      | "411"   ; Length Required
                      | "412"   ; Unless True
                      | "500"   ; Internal Server Error
                      | "501"   ; Not Implemented
                      | "502"   ; Bad Gateway
                      | "503"   ; Service Unavailable
                      | "504"   ; Gateway Timeout
    */

    public class BadRequest : HttpError
    {
        public BadRequest(string errorString)
        {
            ErrorCode = 400;
            ErrorName = "Bad Request";
            ErrorString = errorString;
        }
    }

    public class NotFound : HttpError
    {
        public NotFound(string errorString)
        {
            ErrorCode = 404;
            ErrorName = "Not Found";
            ErrorString = errorString;
        }
    }

    public class NotImplemented: HttpError
    {
        public NotImplemented(string errorString)
        {
            ErrorCode = 501;
            ErrorName = "Not Implemented";
            ErrorString = errorString;
        }
    }
}
