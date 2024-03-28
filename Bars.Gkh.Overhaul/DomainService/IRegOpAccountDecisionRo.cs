namespace Bars.Gkh.Overhaul.DomainService
{
    using System;
    using System.Collections.Generic;
    using Bars.Gkh.Enums.Decisions;

    public interface IRegOpAccountDecisionRo
    {
        Dictionary<long, CrFundFormationDecisionType> GetRobjectFormFundCr(long[] muIds, DateTime endDate);
    }
}