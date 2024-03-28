namespace Bars.Gkh.RegOperator.Exceptions
{
    using System;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    [Serializable]
    public class MoneyOperationException : Exception
    {
        private readonly MoneyOperation _operation;
        public virtual MoneyOperation Operation { get { return _operation; } }

        public MoneyOperationException()
        {

        }

        public MoneyOperationException(string message)
            : base(message)
        {

        }
        public MoneyOperationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public MoneyOperationException(MoneyOperation operation)
        {
            _operation = operation;
        }
    }
}
