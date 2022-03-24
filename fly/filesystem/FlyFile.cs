using System;
using System.Collections.Generic;
using System.Text;

namespace fly.filesystem
{
    class FlyFile
    {
        public FlyFile(string absolutePath, string contentType, byte[] filecontent)
        {
            FileContent = filecontent;
            AbsolutePath = absolutePath;
            ContentType = contentType;
            FileSize = filecontent.Length;
        }
        public string ContentType { get; set; }
        public string AbsolutePath { get; set; }
        public byte[] FileContent { get; set; }
        public int FileSize { get; set; }
    }
}
