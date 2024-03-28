namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States
{
    using System;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    public class ClwUtils
    {
        public static DateTime GetDate(ClaimWorkDocumentType docType, DebtorTypeConfig config, DateTime? startingDate)
        {
            PeriodKind periodKind = PeriodKind.Day;
            int delay = 0;

            return periodKind == PeriodKind.Day
                ? startingDate.GetValueOrDefault().AddDays(delay)
                : startingDate.GetValueOrDefault().AddMonths(delay);
        }
    }
}