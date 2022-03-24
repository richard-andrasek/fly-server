using System;
using System.Collections.Generic;
using System.Text;
using fly.log;

namespace fly.filesystem
{
    class FlyFileCache
    {
        private static FlyFileCache instance;
        private static Object lockObject = new Object();
        private Dictionary<string, FlyFileCacheItem> cache;
        private Lumberjack logger = new Lumberjack("FlyFileCache");
        private static bool _cacheInvalidationInProgress = false;

        private long cacheSize;

        private FlyFileCache()
        {
            cache = new Dictionary<string, FlyFileCacheItem>();
            cacheSize = 0;
            _cacheInvalidationInProgress = false;
        }

        static private FlyFileCache Instance
        {
            get
            {
                if(instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new FlyFileCache();
                        }
                    }
                }
                return instance;
            }
        }

        private void InvalidateCache()
        {
            // chop it down to 70% of max size...
            long maxCache = (long)(config.Configuration.MaxFileCacheSizeInBytes * 0.7);
            DateTime startTime = DateTime.Now;

            //
            // "There are only two hard things in Computer Science: cache invalidation and naming things." ~Steve Jobs
            //


            if (_cacheInvalidationInProgress)
                return;

            lock (lockObject)
            {
                // This can happen when we get stuck at that lock, but invalidation is already in progress...
                if (cacheSize <= maxCache)
                    return;

                //
                // HERE BE DRAGONS!!
                // Be very careful returning early from this lock... 
                // This flag _cacheInvalidationInProgress MUST be set back to false ANY TIME we return.
                //
                _cacheInvalidationInProgress = true;
                logger.Log("Invalidating cache. Current size: " + cacheSize.ToString());

                // Step 1: sort from oldest to newest
                SortedDictionary<DateTime, int> lastAccessHash = new SortedDictionary<DateTime, int>();
                foreach(var kvpair in cache)
                {
                    FlyFileCacheItem item = kvpair.Value;
                    lastAccessHash[item.LastAccess] = item.File.FileSize;
                }

                // Step 2: Figure out how much to chop off.
                DateTime? chopoffDate = null;
                long newCacheSize = 0;
                foreach(var kvpair in lastAccessHash)
                {
                    newCacheSize = kvpair.Value;
                    if (newCacheSize <= maxCache)
                    {
                        chopoffDate = kvpair.Key;
                    }
                    else
                    {
                        // Exceeded new cache size, exit.
                        break;
                    }
                }

                // Step 3: Loop (again) through the cache and drop everything that's too old.
                if (!chopoffDate.HasValue)
                {
                    // This should never happen.  But if it does, just nuke it all and start over.
                    logger.Error("Cache invalidation could not find cutoff for invalidation.  Truncating cache.");
                    logger.Log("Cache invalidation duration (in ticks): " + (DateTime.Now - startTime).Ticks.ToString());
                    cache = new Dictionary<string, FlyFileCacheItem>();
                    _cacheInvalidationInProgress = false;
                    return;
                }

                List<string> itemsToRemove = new List<string>();
                int count = 0;
                long bytesRemoved = 0;
                foreach(var kvpair in cache)
                {
                    // First, find all the items that are "too old" and add them to a "hit list"
                    FlyFileCacheItem item = kvpair.Value;
                    if (item.LastAccess < chopoffDate)
                    {
                        itemsToRemove.Add(kvpair.Key);
                        count++;
                        bytesRemoved += item.File.FileSize;
                    }
                }

                logger.Log("Invalidating item count: " + count.ToString());
                foreach(var key in itemsToRemove)
                {
                    // Drop everything from the "hit list"
                    cache.Remove(key);
                }
                cacheSize -= bytesRemoved;
            }
            _cacheInvalidationInProgress = false;
            long ticks = (DateTime.Now - startTime).Ticks;
            logger.Log("Cache invalidation duration (in ticks): " + ticks.ToString());
        }

        private FlyFile GetFileFromCache(string absolutePath)
        {
            lock(lockObject)
            {
                if (cache.ContainsKey(absolutePath))
                {
                    logger.Log("File found in cache: " + absolutePath);
                    return cache[absolutePath].File;
                }
            }
            return null;
        }

        private bool AddFileToCache(FlyFile file)
        {
            long maxCache = config.Configuration.MaxFileCacheSizeInBytes;

            // Too big to cache
            if (file.FileSize > (maxCache * 0.8))
            {
                logger.Log("Could not cache file.  File too large: " + file.AbsolutePath);
                return false;
            }

            logger.Log("Adding file to cache: " + file.AbsolutePath);
            // Add the file to the cache
            lock (lockObject)
            {
                if (cache.ContainsKey(file.AbsolutePath))
                {
                    return true;
                }
                // Note: this doesn't take into account the size of the objects, but that should be negligible. 
                cacheSize += file.FileSize;

                FlyFileCacheItem cacheItem = new FlyFileCacheItem();
                cacheItem.LastAccess = DateTime.Now;
                cacheItem.File = file;
                cache[file.AbsolutePath] = cacheItem;
                logger.Log("Cache size: " + cacheSize.ToString());
            }

            // If we're exceeding our max cache size, invalidate some of the files
            if (cacheSize > config.Configuration.MaxFileCacheSizeInBytes)
            {
                InvalidateCache();
            }
            return true;
        }

        public static bool TryRetrieveFile(string uri, out FlyFile file)
        {
            file = Instance.GetFileFromCache(uri);
            if (file == null) return false;
            return true;
        }

        public static void CacheFile(FlyFile file)
        {
            Instance.AddFileToCache(file);
        }
    }

}
