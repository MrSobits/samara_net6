namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.ReferenceCalculation
{
    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
    using System.Collections.Generic;

    public interface IDebtorReferenceCalculationService
    {
        DebtCalc DebtCalc { get; }

        PenaltyCharge PenaltyCharge { get; }

        IDataResult CalculateReferencePayments(AgentPIRDebtor debtor, List<long> transfers);
    }
}