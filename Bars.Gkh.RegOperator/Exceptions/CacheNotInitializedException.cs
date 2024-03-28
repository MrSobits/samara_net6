namespace Bars.Gkh.RegOperator.Exceptions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CacheNotInitializedException : Exception
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CacheNotInitializedException(string cacheName) : base(string.Format("{0} is not initialized", cacheName))
        {
            
        }
    }
}