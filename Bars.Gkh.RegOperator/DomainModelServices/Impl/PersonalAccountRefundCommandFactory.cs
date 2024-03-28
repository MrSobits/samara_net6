namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;

    using Refund;
    using Enums;

    /// <summary>
    /// Фабрика для получения реализаций IPersonalAccountRefundCommand
    /// </summary>
    public class PersonalAccountRefundCommandFactory : IPersonalAccountRefundCommandFactory
    {
        /// <summary>
        /// Выбор команды в зависимости от ImportedPaymentType
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public IPersonalAccountRefundCommand GetCommand(ImportedPaymentType paymentType)
        {
            switch (paymentType)
            {
                case ImportedPaymentType.Refund:
                    return new PersonalAccountRefundCommand(RefundType.CrPayments);
                case ImportedPaymentType.PenaltyRefund:
                    return new PersonalAccountRefundCommand(RefundType.Penalty);
            }

            return null;
        }
    }
}
