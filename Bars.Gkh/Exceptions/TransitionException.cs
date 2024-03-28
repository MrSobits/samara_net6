namespace Bars.Gkh.Exceptions
{
    using System;

    public class TransitionException: Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public TransitionException()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public TransitionException(string message) : base(message)
        {
            
        }
    }
}
