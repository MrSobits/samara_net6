namespace Bars.Gkh.RegOperator.Exceptions
{
    using System;

    [Serializable]
    public class RefundException: Exception
    {
        public RefundException()
        {
            
        }

        public RefundException(string message) : base(message)
        {
            
        }
    }
}
