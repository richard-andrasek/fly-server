using System;
using System.Collections.Generic;
using System.Text;
using fly.filesystem;

namespace fly.config
{
    class Configuration
    {
        static private Configuration _instance = null;
        static private Object _instanceLock = new Object();
        public Configuration()
        {
            // Milestone 1: Get config (done)
            // Milestone 2: Load from file with defaults

            _FlyVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            // For a full list, see here:
            // https://www.geeksforgeeks.org/http-headers-content-type/
            // (Not all of those should be supported, though).
            _FileExtensionToContentTypeMap = new Dictionary<string, ContentType> {
                { ".html", new ContentType("text/html", "ascii") },
                { ".htm", new ContentType("text/html", "ascii")},
                { ".js", new ContentType("application/javascript", "ascii")},
                { ".tsx", new ContentType("application/javascript", "ascii")},
                { ".jpg", new ContentType("image/jpeg", "base64") },
                { ".jpeg", new ContentType("image/jpeg", "base64") },
                { ".png", new ContentType("image/png", "base64") },
                { ".gif", new ContentType("image/gif", "base64") },
                { ".webp", new ContentType("image/webp", "base64") },
                { ".svg", new ContentType("image/svg+xml", "ascii") }, // Not sure about this one...
                { ".bmp", new ContentType("image/bmp", "base64") },
                { ".ico", new ContentType("image/x-icon", "base64") },
                { ".cur", new ContentType("image/x-icon", "base64") },
                { ".tif", new ContentType("image/tiff", "base64") },
                { ".tiff", new ContentType("image/tiff", "base64") },
                { ".pdf", new ContentType("application/pdf", "base64") },
                { ".txt", new ContentType("text/plain", "ascii") },
                { ".css", new ContentType("text/css", "ascii") }
            };
            _DefaultHost = "127.0.0.1";
            _DefaultPort = 80;
            _NumberOfWorkerThreads = 16;
            _MaxFileCacheSizeInBytes = 1073741824; // 1 GB
        }

        static private Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Configuration();
                        }
                    }
                }
                return _instance;
            }
        }
        /*
         * Private (Singleton) Member Methods
         */
        private string _FlyVersion { get; set; }
        private Dictionary<string, ContentType> _FileExtensionToContentTypeMap { get; set; }
        private string _DefaultHost { get; set; }
        private int _DefaultPort { get; set; }
        private int _NumberOfWorkerThreads { get; set; }
        private long _MaxFileCacheSizeInBytes { get; set; }

        /*
         * Public (Static) Member Methods
         */
        static public string ServerVersion { get {  return Instance._FlyVersion; } }
        static public Dictionary<string, ContentType> FileExtensionToContentTypeMap {  get { return Instance._FileExtensionToContentTypeMap; } }
        static public string DefaultHost { get { return Instance._DefaultHost; } }
        static public int DefaultPort { get { return Instance._DefaultPort; } }
        static public int NumberOfWorkerThreads {  get { return Instance._NumberOfWorkerThreads;  } }
        static public long MaxFileCacheSizeInBytes {  get { return Instance._MaxFileCacheSizeInBytes; } }

        static public string GetConfiguration(string keyName)
        {
            if(keyName == "defaultIndexFile") { return "index.html"; }

            return null;
        }
    }
}
