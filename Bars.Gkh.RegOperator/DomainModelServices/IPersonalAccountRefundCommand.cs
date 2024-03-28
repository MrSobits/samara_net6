namespace Bars.Gkh.RegOperator.DomainModelServices
{
	using System;

	using Bars.Gkh.Entities;

	using Domain;
    using Domain.ValueObjects;
    using Entities;
    using Entities.ValueObjects;

    public interface IPersonalAccountRefundCommand
    {
        PersonalAccountRefundResult Execute(BasePersonalAccount account, MoneyStream source, ExecutionMode mode = ExecutionMode.Sequential);

        PersonalAccountRefundResult Undo(BasePersonalAccount account, MoneyOperation operation, ChargePeriod period, DateTime? cancelDate = null);

        decimal GetAccountBalance(BasePersonalAccount account);
    }
}
