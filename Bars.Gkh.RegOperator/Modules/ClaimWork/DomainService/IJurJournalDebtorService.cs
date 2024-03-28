namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService
{
    using System.Collections;
    using B4;

    public interface IJurJournalDebtorService
    {
        IList GetList(BaseParams baseParams, bool usePaging, out int totalCount);
    }
}