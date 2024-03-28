namespace Bars.B4.Modules.Analytics.Exceptions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DataProviderNotSpecifiedException : Exception
    {
        public DataProviderNotSpecifiedException()
            : base()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public DataProviderNotSpecifiedException(string message)
            : base(message)
        {

        }
    }
}
