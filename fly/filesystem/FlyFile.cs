using System;
using System.Collections.Generic;
using System.Text;

namespace fly.filesystem
{
    class FlyFile
    {
        public FlyFile(string absolutePath, string contentType, byte[] filecontent, string encoding)
        {
            FileContent = filecontent;
            AbsolutePath = absolutePath;
            ContentEncoding = encoding;
            ContentType = contentType;
        }
        public string ContentType { get; set; }
        public string AbsolutePath { get; set; }
        public string ContentEncoding { get; set; }
        public byte[] FileContent { get; set; }
    }
}
