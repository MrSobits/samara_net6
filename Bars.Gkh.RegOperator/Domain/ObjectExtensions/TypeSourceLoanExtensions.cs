namespace Bars.Gkh.RegOperator.Domain.ObjectExtensions
{
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Расширения для перевода между типами Источник займа - Тип оплаты акта КР
    /// </summary>
    public static class TypeSourceLoanExtensions
    {
        /// К Типy оплаты акта КР из Источника займа

        public static ActPaymentSrcFinanceType ToActPaymentSrcFinanceType(this TypeSourceLoan typeSourceLoan)
        {
            switch (typeSourceLoan)
            {
                case TypeSourceLoan.PaymentByMinTariff:
                    return ActPaymentSrcFinanceType.OwnerFundByMinTarrif;

                case TypeSourceLoan.FundSubsidy:
                    return ActPaymentSrcFinanceType.FundSubsidy;

                case TypeSourceLoan.RegionalSubsidy:
                    return ActPaymentSrcFinanceType.RegionSubsidy;

                case TypeSourceLoan.StimulateSubsidy:
                    return ActPaymentSrcFinanceType.StimulSubsidy;

                case TypeSourceLoan.TargetSubsidy:
                    return ActPaymentSrcFinanceType.TargetSubsidy;

                case TypeSourceLoan.PaymentByDesicionTariff:
                    return ActPaymentSrcFinanceType.OwnerFundByDecisionTariff;

                case TypeSourceLoan.Penalty:
                    return ActPaymentSrcFinanceType.Penalty;
            }

            return ActPaymentSrcFinanceType.Other;
        }
    }
}