namespace Bars.B4.Modules.Analytics.Exceptions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DataProviderNotFoundException: Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public DataProviderNotFoundException() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DataProviderNotFoundException(string message) : base(message)
        {
        }
    }
}
