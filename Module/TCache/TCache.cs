using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TCache
{
    public class TCache
    {
        public static bool AddObject(string key, object value, DateTimeOffset timeOffset, bool del)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return false;
                if (del)
                    DeleteObject(key);
                return MemoryCache.Default.Add(key, value, timeOffset);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteObject(string key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                if (memoryCache.Contains(key))
                    memoryCache.Remove(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object GetObject(string key)
        {
            try
            {
                return MemoryCache.Default.Get(key);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
