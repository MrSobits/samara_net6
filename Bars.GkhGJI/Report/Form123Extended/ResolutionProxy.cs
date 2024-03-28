using System;

namespace Bars.GkhGji.Report.Form123Extended
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    internal sealed class ResolutionProxy
    {
        public long Id;
        public TypeInitiativeOrgGji TypeInitiativeOrg;
        public string ExecutantCode;
        public string SanctionCode;
        public Decimal? PenaltyAmount;
        public YesNoNotSet Paided;
    }
}
