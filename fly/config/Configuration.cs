﻿using System;
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
            _FileExtensionToContentTypeMap = new Dictionary<string, ContentType> {
                { ".html", new ContentType("text/html", "ascii") },
                { ".htm", new ContentType("text/html", "ascii")},
                { ".js", new ContentType("application/javascript", "ascii")},
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
                { ".tiff", new ContentType("image/tiff", "base64") }
            };
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
        private string _FlyVersion { get; set; }
        private Dictionary<string, ContentType> _FileExtensionToContentTypeMap { get; set; }

        static public string ServerVersion { get {  return Instance._FlyVersion; } }
        static public Dictionary<string, ContentType> FileExtensionToContentTypeMap {  get { return Instance._FileExtensionToContentTypeMap; } }

        static public string GetConfiguration(string keyName)
        {
            if(keyName == "defaultIndexFile") { return "index.html"; }

            return null;
        }
    }
}
