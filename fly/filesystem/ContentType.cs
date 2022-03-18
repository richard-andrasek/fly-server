using System;
using System.Collections.Generic;
using System.Text;

namespace fly.filesystem
{
    class ContentType
    {
        public ContentType(string name, string encoding)
        {
            Name = name;
            Encoding = encoding;
        }
        public string Name { get; set; }
        public string Encoding { get; set; }
    }
}
