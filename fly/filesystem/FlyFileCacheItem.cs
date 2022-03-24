using System;
using System.Collections.Generic;
using System.Text;

namespace fly.filesystem
{
    class FlyFileCacheItem
    {
        public FlyFile File { get; set; }
        public DateTime LastAccess { get; set; }
    }
}
