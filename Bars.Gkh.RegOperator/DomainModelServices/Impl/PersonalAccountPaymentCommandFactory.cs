namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using PersonalAccountPayment;
    using Enums;

    /// <summary>
    /// Фабрика для получения реализаций IPersonalAccountPaymentCommand
    /// </summary>
    public class PersonalAccountPaymentCommandFactory : IPersonalAccountPaymentCommandFactory
    {
        /// <summary>
        /// Выбор команды в зависимости от PaymentType
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public IPersonalAccountPaymentCommand GetCommand(PaymentType paymentType)
        {
            switch (paymentType)
            {
                case PaymentType.Basic: 
                    return new PersonalAccountTariffPaymentCommand();
                case PaymentType.Penalty:
                    return new PersonalAccountPenaltyPaymentCommand();
                case PaymentType.SocialSupport:
                    return new PersonalAccountSocialSupportPaymentCommand();
                case PaymentType.Rent:
                    return new PersonalAccountRentPaymentCommand();
                case PaymentType.PreviousWork:
                    return new PersonalAccountPreviosWorkPaymentCommand();
                case PaymentType.AccumulatedFund:
                    return new PersonalAccountAccumulatedFundPaymentCommand();
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Выбор команды в зависимости от ImportedPaymentType
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public IPersonalAccountPaymentCommand GetCommand(ImportedPaymentType paymentType)
        {
            switch (paymentType)
            {
                case ImportedPaymentType.Basic:
                case ImportedPaymentType.Payment:
                case ImportedPaymentType.ChargePayment:
                case ImportedPaymentType.Sum:
                    return new PersonalAccountTariffPaymentCommand();
                case ImportedPaymentType.Penalty:
                    return new PersonalAccountPenaltyPaymentCommand();
                case ImportedPaymentType.SocialSupport:
                    return new PersonalAccountSocialSupportPaymentCommand();
            }

            throw new NotSupportedException();
        }
    }
}