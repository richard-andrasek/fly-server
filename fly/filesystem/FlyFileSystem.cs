using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using fly.log;
using fly.config;
using fly.http_errors;
using fly.config;

namespace fly.filesystem
{
    class FlyFileSystem
    {
        private static FlyFileSystem _instance = null;
        private static readonly object instancelock = new object();

        // Private constructor so that it can't be created outside of this class
        private FlyFileSystem()
        {
            // TODO: Milestones
            // Milestone 1: Read a file... that's it.  Just read it.  (done)
            // Milestone 2: Process all types of files (done)
            // Milestone 3: Allow for default file (404.html)
            // Milestone 4: Caching (disabled by config, max size, etc.)
            // Milestone 5: File Streaming (mp4, mp3, etc.)
        }

        private string _root = null;
        public string RootDirectory()
        {
            if (_root == null)
            {
                _root = Directory.GetCurrentDirectory();
            }
            return _root;
        }

        public static FlyFileSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (instancelock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FlyFileSystem();
                        }
                    }
                }
                return _instance;
            }
        }

        public static bool TryRetrieveByURI(string uri, out FlyFile outFile, out HttpError errorResponse)
        {
            outFile = null;
            errorResponse = null;

            // Get the Absolute Path
            // (This is the path from the root directory to the file)
            string absolutePath;
            HttpError conversionError;
            if (!TryConvertUriToAbsolutePath(uri, out absolutePath, out conversionError))
            {
                errorResponse = conversionError;
                return false;
            }

            // Find the file on disk
            string fullFilePath = Instance.RootDirectory() + absolutePath;
            FileInfo finfo = new FileInfo(fullFilePath);
            if(!finfo.Exists)
            {
                errorResponse = new NotFound("File Not Found");
                return false;
            }

            // Get the Content Type info
            string contentType = FileToContentType(finfo);

            // Get the Contents of the file
            byte[] fileBytes = File.ReadAllBytes(fullFilePath);

            
            // Create the FlyFile
            outFile = new FlyFile(absolutePath, contentType, fileBytes);

            return true;
        }


        // Strip off the non-useful parts of the URI to get the absolute path from the root to the file.
        private static bool TryConvertUriToAbsolutePath(string uri, out string absolutePath, out HttpError returnError)
        {
            returnError = null;
            absolutePath = null;
            // ------------------------------------------
            // This is the most fragile part of the server
            // I believe that there are MANY edge cases that are not covered
            // See: https://www.w3.org/Protocols/HTTP/1.1/draft-ietf-http-v11-spec-01.html#URI
            // ------------------------------------------

            //  TODO:  test this...
            //          http://../../../

            string full_url;
            if (uri.StartsWith("http"))
            {
                full_url = uri;
            }
            else 
            {
                // NOTE: This address is PURELY for the URI converter (even the protocol doesn't matter here)
                full_url = "http://0.0.0.0" + uri;
                if (!uri.StartsWith('/'))
                {
                    full_url += "http://0.0.0.0/" + uri;
                }
            }

            // Convert the full URL to a "Uri" object (let C# do the work)
            if (!Uri.IsWellFormedUriString(full_url, UriKind.RelativeOrAbsolute))
            {
                returnError = new BadRequest("Invalid URL");
                return false;
            }
            Uri fileUri = new Uri(full_url, UriKind.RelativeOrAbsolute);

            // Finally, get the Absoulte Path (stripping off the query string)
            if(fileUri.AbsolutePath == null)
            {
                returnError = new BadRequest("Invalid URL");
                return false;
            }
            absolutePath = fileUri.AbsolutePath;

            // Use the Default Index File (index.html)
            if (absolutePath.EndsWith('/'))
            {
                // TODO: Consider using a redirect here, rather than a re-write (make it a configuration?)
                absolutePath += Configuration.GetConfiguration("defaultIndexFile");
            }
            return true;
        }

        private static string FileToContentType(FileInfo fileInfo)
        {
            string ext = fileInfo.Extension.ToLower();

            if(Configuration.FileExtensionToContentTypeMap.ContainsKey(ext))
            {
                ContentType type = Configuration.FileExtensionToContentTypeMap[ext];
                return type.Name;
            }

            // Did not find file type... Log it and default
            Lumberjack logger = new Lumberjack("FlyFileSystem");
            logger.Error("Returning default content type for unknown file extension: [" + ext + "]");

            // TODO: Once all other files are in place, this REALLY needs to be set to "octet"
            return "text/plain";
        }
    }
}
