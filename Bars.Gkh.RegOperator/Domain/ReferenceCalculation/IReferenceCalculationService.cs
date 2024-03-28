namespace Bars.Gkh.RegOperator.Domain.ReferenceCalculation
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;

    public interface IReferenceCalculationService
    {
        DebtCalc DebtCalc { get; }

        PenaltyCharge PenaltyCharge { get; }

        IDataResult CalculateReferencePayments(Lawsuit lawsuit, List<ClaimWorkAccountDetail> claimWorkAccountDetailList, List<long> transfers);
    }
}