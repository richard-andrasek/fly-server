# Fly Server
Fly is an ultra-lightweight HTTP server for serving static files.  

It processes GET requests and serves them with **sub-millisecond speed**.

## What it can do
Quite simply, Fly serves static pages quickly.

## What it cannot do
This is a static server only!  It cannot handle any server-side processing (PHP, ASP.Net, Ruby, etc).  Also, because of this, it's limited exclusively to GET requests.

By not processing requests server-side, Fly is able to serve pages at a blazing fast speed (sub-millisecond).

Currently, there's no SSL/TLS support, so no HTTPS connections.

## Why?
My goal is to make an HTTP server that does one thing exceedingly well: serve static pages.

The concept for this came from the SFTP server that I wrote previously.  SFTP is Simple FTP, which contains a small subset of the standard FTP protocol (no security, for example).  So, this is, effectively, an SHTTP server: a simple HTTP server.  It only supports the GET method.  It's perfect for serving static files.