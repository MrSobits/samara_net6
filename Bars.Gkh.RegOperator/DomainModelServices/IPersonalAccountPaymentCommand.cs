namespace Bars.Gkh.RegOperator.DomainModelServices
{
	using System;

	using Bars.Gkh.Entities;

	using Domain;
    using Domain.ValueObjects;
    using Entities;
    using Entities.ValueObjects;
    using Enums;

    public interface IPersonalAccountPaymentCommand
    {
        PersonalAccountPaymentResult Execute(BasePersonalAccount account, MoneyStream source, AmountDistributionType type = AmountDistributionType.Tariff, AccountDistributionMoneyReserve reserve = null);

        PersonalAccountPaymentUndoResult Undo(BasePersonalAccount account, MoneyOperation operation, ChargePeriod period, DateTime? cancelDate = null);
    }
}