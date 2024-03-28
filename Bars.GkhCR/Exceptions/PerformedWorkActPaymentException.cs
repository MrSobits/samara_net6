namespace Bars.GkhCr.Exceptions
{
    using System;
    using Bars.GkhCr.Entities;

    [Serializable]
    public class PerformedWorkActPaymentException : Exception
    {
        private readonly PerformedWorkActPayment _payment;
        public PerformedWorkActPayment Payment { get { return _payment; } }

        public PerformedWorkActPaymentException(PerformedWorkActPayment payment)
            : base()
        {
            _payment = Payment;
        }

        public PerformedWorkActPaymentException()
        {
        }

        public PerformedWorkActPaymentException(string message)
            : base(message)
        {

        }

    }
}
