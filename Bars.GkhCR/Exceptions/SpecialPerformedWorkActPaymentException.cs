namespace Bars.GkhCr.Exceptions
{
    using System;
    using Bars.GkhCr.Entities;

    [Serializable]
    public class SpecialPerformedWorkActPaymentException : Exception
    {
        private readonly SpecialPerformedWorkActPayment _payment;
        public SpecialPerformedWorkActPayment Payment { get { return this._payment; } }

        public SpecialPerformedWorkActPaymentException(SpecialPerformedWorkActPayment payment)
            : base()
        {
            this._payment = this.Payment;
        }

        public SpecialPerformedWorkActPaymentException()
        {
        }

        public SpecialPerformedWorkActPaymentException(string message)
            : base(message)
        {

        }

    }
}
