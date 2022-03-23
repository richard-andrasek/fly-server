# fly-server
An ultra-lightweight HTTP server for serving static files


## What it can do
This server serves static pages quickly.

## What it cannot do
This is a static server only.  It cannot handle any server-side processing (PHP, ASP.Net, Ruby, etc).  

Also, because of this, it's limited exclusively to GET requests.

Currently, there's no SSL/TLS support, so no HTTPS connections.

## Why?
It's a side project.  I wanted to create a very simple http server.  My goal is to make it do one thing well: serve static pages.